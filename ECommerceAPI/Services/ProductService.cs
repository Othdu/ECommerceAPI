using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class ProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public PagedResult<ProductResponseDto> GetAll(PaginationParams p)
        {
            var query = _db.Products
                .Include(pr => pr.Category)
                .AsQueryable();

            // Apply search
            if (!string.IsNullOrEmpty(p.Search))
                query = query.Where(pr => pr.Name!.Contains(p.Search));

            // Apply sorting
            query = p.SortBy?.ToLower() switch
            {
                "price" => p.SortDescending
                    ? query.OrderByDescending(pr => pr.Price)
                    : query.OrderBy(pr => pr.Price),
                "name" => p.SortDescending
                    ? query.OrderByDescending(pr => pr.Name)
                    : query.OrderBy(pr => pr.Name),
                _ => query.OrderBy(pr => pr.Id)
            };

            // Count total BEFORE pagination
            var totalCount = query.Count();

            // Apply pagination
            var products = query
                .Skip((p.Page - 1) * p.PageSize)
                .Take(p.PageSize)
                .Select(pr => new ProductResponseDto
                {
                    Id = pr.Id,
                    Name = pr.Name,
                    Description = pr.Description,
                    Price = pr.Price,
                    Stock = pr.Stock,
                    CategoryName = pr.Category!.Name
                })
                .ToList();

            return new PagedResult<ProductResponseDto>
            {
                Data = products,
                Page = p.Page,
                PageSize = p.PageSize,
                TotalCount = totalCount
            };
        }

        public ProductResponseDto? GetById(int id)
        {
            var product = _db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category!.Name
            };
        }

        public ProductResponseDto? Create(ProductDto dto)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == dto.CategoryId);
            if (category == null) return null;

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };

            _db.Products.Add(product);
            _db.SaveChanges();

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = category.Name
            };
        }

        public bool Update(int id, ProductDto dto)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;
            _db.Products.Remove(product);
            _db.SaveChanges();
            return true;
        }
    }
}