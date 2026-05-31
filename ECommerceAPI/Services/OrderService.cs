using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Services
{
    public class OrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public OrderResponseDto? Create(CreateOrderDto dto, int userId)
        {
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            double total = 0;

            foreach (var item in dto.Items!)
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null) return null;
                if (product.Stock < item.Quantity) return null;

                product.Stock -= item.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
                total += product.Price * item.Quantity;
            }

            order.TotalAmount = total;
            _db.Orders.Add(order);
            _db.SaveChanges();

            return MapToDto(order);
        }

        public List<OrderResponseDto> GetUserOrders(int userId)
        {
            return _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .Select(o => MapToDto(o))
                .ToList();
        }

        public OrderResponseDto? GetById(int id, int userId)
        {
            var order = _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (order == null) return null;
            return MapToDto(order);
        }

        private static OrderResponseDto MapToDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems?.Select(oi => new OrderItemResponseDto
                {
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Subtotal = oi.Quantity * oi.UnitPrice
                }).ToList()
            };
        }
    }
}