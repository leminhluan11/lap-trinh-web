using FashionEcommerce.Data;
using FashionEcommerce.Models;
using FashionEcommerce.Services;
using FashionEcommerce.DTOs;  // Thêm using này để dùng DTO
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
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Các trường bắt buộc không được để trống"
                });
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (existingUser != null)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Tên đăng nhập đã tồn tại"
                });
            }

            string role = request.Role == "Admin" ? "Admin" : "Customer";

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                GoogleId = request.GoogleId,
                DateOfBirth = request.DateOfBirth,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                AvatarUrl = request.AvatarUrl,
                Role = role,
                IsLocked = false,
                CreatedAt = DateTime.UtcNow  // Nên dùng UTC để tránh lệch múi giờ
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                Success = true,
                Message = "Đăng ký thành công",
                UserId = newUser.Id,
                Username = newUser.Username,
                Role = newUser.Role
                // Không trả token ở register
            };

            return Ok(response);
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
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Tên đăng nhập và mật khẩu là bắt buộc"
                });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                });
            }

            if (user.IsLocked)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Tài khoản đã bị khóa"
                });
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValidPassword)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                });
            }

            string token = _jwt.GenerateToken(user.Username!, user.Role!, user.Id);

            var response = new AuthResponseDto
            {
                Success = true,
                Message = "Đăng nhập thành công",
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role,
                Token = token
            };

            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Với JWT, logout thường chỉ cần client xóa token
            // Nếu bạn dùng refresh token hoặc blacklist, có thể thêm logic ở đây
            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Đăng xuất thành công"
            });
        }
    }

    // =========================
    // DTOs (Request) - giữ nguyên như bạn
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

        public string? GoogleId { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }

        public string Role { get; set; } = "Customer";
    }
}