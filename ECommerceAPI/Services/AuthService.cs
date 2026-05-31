using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db , IConfiguration config)
        {
            _config = config;
            _db = db;
        }

        public AuthResponseDto? Register(RegisterDto dto)
        {
            if (_db.Users.Any(u=> u.Email==dto.Email))return null;
            var user = new User
            {
                Email = dto.Email,
                PasswordHash= HashPassword(dto.Password!),
                Role ="User"
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            return new AuthResponseDto
            {
                Token = GenerateToken(user),
                Email = user.Email,
                Role = user.Role,
            };
        }
        public AuthResponseDto? Login(LoginDto dto)
        {
            var user = _db.Users.FirstOrDefault(U=>U.Email== dto.Email);
            if (user == null) return null;
            if (!VerifyPassword(dto.Password!, user.PasswordHash)) return null;
            return new AuthResponseDto
            {
                Token = GenerateToken(user),
                Email = user.Email,
                Role =user.Role,
            };

        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
        private bool VerifyPassword(string password, string hash) => HashPassword(password) == hash;

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Role,user.Role!),
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
