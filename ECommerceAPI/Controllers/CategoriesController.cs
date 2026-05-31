using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_categoryService.GetAll());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CategoryDto dto)
        {
            var result = _categoryService.Create(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (!_categoryService.Delete(id)) return NotFound();
            return NoContent();
        }
    }
}