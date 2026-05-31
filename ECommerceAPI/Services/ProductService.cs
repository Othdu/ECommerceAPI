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

        public List<ProductResponseDto> GetAll()
        {
            return _db.Products
                .Include(p => p.Category)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = p.Category!.Name
                })
                .ToList();
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