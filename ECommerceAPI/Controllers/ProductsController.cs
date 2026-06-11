using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] PaginationParams p)
        {
            return Ok(_productService.GetAll(p));
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ProductDto dto)
        {
            var result = _productService.Create(dto);
            if (result == null) return BadRequest("Category not found.");
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, ProductDto dto)
        {
            if (!_productService.Update(id, dto)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            if (!_productService.Delete(id)) return NotFound();
            return NoContent();
        }
    }
}