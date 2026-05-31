using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class ProductDto
    {
        [Required][StringLength(200,MinimumLength =2)]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock cant be in negative")]
        public int Stock { get; set; }
        [Required]
        public int CategoryId { get; set; } 
    }
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string ? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set;  }
        public string? CategoryName { get; set; }

    }

}
