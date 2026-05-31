using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult GetStats()
        {
            var totalUsers = _db.Users.Count();
            var totalOrders = _db.Orders.Count();
            var totalRevenue = _db.Orders.Sum(o => o.TotalAmount);
            var totalProducts = _db.Products.Count();

            var topProducts = _db.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => oi.Product!.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(5)
                .ToList();

            var recentOrders = _db.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .Select(o => new
                {
                    o.Id,
                    o.TotalAmount,
                    o.CreatedAt,
                    UserEmail = o.User!.Email
                })
                .ToList();

            return Ok(new
            {
                TotalUsers = totalUsers,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalProducts = totalProducts,
                TopProducts = topProducts,
                RecentOrders = recentOrders
            });
        }
    }
}