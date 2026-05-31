using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public ActionResult GetMyOrders()
        {
            return Ok(_orderService.GetUserOrders(GetUserId()));
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var order = _orderService.GetById(id, GetUserId());
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public ActionResult Create(CreateOrderDto dto)
        {
            var result = _orderService.Create(dto, GetUserId());
            if (result == null) return BadRequest("One or more products are unavailable or out of stock.");
            return Ok(result);
        }
    }
}