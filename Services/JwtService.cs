using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FashionEcommerce.Services
{
    public interface IJwtService
    {
        string GenerateToken(string username, string role, int userId);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string username, string role, int userId)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured")
                )
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim("Id", userId.ToString())
            };

            // Lấy thời gian hết hạn từ appsettings
            int expiresMinutes = int.Parse(
                _config["Jwt:ExpiresInMinutes"] ?? throw new InvalidOperationException("Jwt:ExpiresInMinutes not configured")
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer not configured"),
                audience: _config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience not configured"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes), // dùng config
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}