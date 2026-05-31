using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public List<OrderItemDto>? Items { get; set; }
    }

    public class OrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderItemResponseDto>? Items { get; set; }
    }

    public class OrderItemResponseDto
    {
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Subtotal { get; set; }
    }
}