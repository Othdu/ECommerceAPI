using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class RegisterDto

    {
        [Required][EmailAddress]
        public string? Email { get; set; }
        [Required][MinLength(6)]
        public string? Password { get; set; }

    }
    public class LoginDto
    {
        [Required][EmailAddress]
        public string? Email { get; set; }
        [Required][MinLength (6)]
        public string? Password { get; set; }
    }

    public class AuthResponseDto
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
    }
}
