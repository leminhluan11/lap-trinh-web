using FashionEcommerce.Data;
using FashionEcommerce.Models;
using FashionEcommerce.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly FashionEcommerceDbContext _context;

        public AuthController(IJwtService jwt, FashionEcommerceDbContext context)
        {
            _jwt = jwt;
            _context = context;
        }

        // =========================
        // REGISTER
        // =========================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { success = false, message = "Required fields are missing" });
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (existingUser != null)
                return BadRequest(new { success = false, message = "Username already exists" });

            string role = request.Role == "Admin" ? "Admin" : "Customer";

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                GoogleId = request.GoogleId,            // thêm
                DateOfBirth = request.DateOfBirth,      // thêm
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                AvatarUrl = request.AvatarUrl,
                Role = role,
                IsLocked = false,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Register successful",
                userId = newUser.Id,
                role = newUser.Role
            });
        }

        // =========================
        // LOGIN
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { success = false, message = "Username and password are required" });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Unauthorized(new { success = false, message = "Invalid username or password" });

            if (user.IsLocked)
                return Unauthorized(new { success = false, message = "Account is locked" });

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValidPassword)
                return Unauthorized(new { success = false, message = "Invalid username or password" });

            string token = _jwt.GenerateToken(user.Username, user.Role, user.Id);

            return Ok(new
            {
                success = true,
                data = new
                {
                    userId = user.Id,
                    username = user.Username,
                    role = user.Role,
                    token
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { success = true, message = "Logout successful" });
        }
    }

    // =========================
    // DTOs
    // =========================
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? GoogleId { get; set; }       // thêm
        public DateTime? DateOfBirth { get; set; }  // thêm

        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }

        public string Role { get; set; } = "Customer";
    }
}