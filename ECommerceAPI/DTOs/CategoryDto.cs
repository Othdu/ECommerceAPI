using System.ComponentModel.DataAnnotations;
namespace ECommerceAPI.DTOs
{
    public class CategoryDto
    {
        [Required][StringLength(100,MinimumLength =2)]
        public string? Name { get; set; }
    }

    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string ? Name { get; set; }
    }
}
