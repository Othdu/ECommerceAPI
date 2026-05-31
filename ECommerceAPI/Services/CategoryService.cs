using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public List<CategoryResponseDto> GetAll()
        {
            return _db.Categories
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        public CategoryResponseDto? Create(CategoryDto dto)
        {
            var category = new Category { Name = dto.Name };
            _db.Categories.Add(category);
            _db.SaveChanges();

            return new CategoryResponseDto { Id = category.Id, Name = category.Name };
        }

        public bool Delete(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return false;
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return true;
        }
    }
}