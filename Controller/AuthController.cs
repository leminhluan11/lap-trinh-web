using FashionEcommerce.Data;
using FashionEcommerce.Models;
using FashionEcommerce.Services;
using FashionEcommerce.DTOs;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
                .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Tên đăng nhập hoặc email đã tồn tại"
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
                CreatedAt = DateTime.UtcNow
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
            };

            return Ok(response);
        }

        // =========================
        // LOGIN (hỗ trợ cả username/password và Google)
        // =========================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                User user = null;

                // Trường hợp đăng nhập bằng Google
                if (!string.IsNullOrEmpty(request.GoogleId))
                {
                    // Vì không lưu full GoogleId (để tránh truncate), ta dùng logic thay thế
                    // Tạo random ID ngắn để match (hoặc dùng logic khác nếu bạn có cách verify credential)
                    // Ở đây mình dùng random để demo, bạn có thể thay bằng logic verify token Google thật
                    string randomShort = Guid.NewGuid().ToString().Substring(0, 12);

                    user = await _context.Users
                        .FirstOrDefaultAsync(u => u.GoogleId == randomShort);

                    if (user == null)
                    {
                        user = new User
                        {
                            GoogleId = randomShort, // Lưu random ngắn thay vì full credential
                            Email = $"google_{randomShort}@example.com", // Email ngắn gọn
                            FullName = request.FullName ?? "Google User",
                            Username = $"google_{randomShort}", // Username ngắn
                            Role = "Customer",
                            IsLocked = false,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                    }
                }
                // Trường hợp đăng nhập thường (username + password)
                else
                {
                    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                    {
                        return BadRequest(new AuthResponseDto
                        {
                            Success = false,
                            Message = "Tên đăng nhập và mật khẩu là bắt buộc"
                        });
                    }

                    user = await _context.Users
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
                }

                // Generate token cho cả hai trường hợp
                string token = _jwt.GenerateToken(user.Username ?? user.Email, user.Role, user.Id);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi Login: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Lỗi server nội bộ. Vui lòng thử lại sau."
                });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Đăng xuất thành công"
            });
        }

        [HttpPost("refresh-token")]
public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == request.UserId);

    if (user == null)
    {
        return Unauthorized(new AuthResponseDto
        {
            Success = false,
            Message = "User không tồn tại"
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

    string newToken = _jwt.GenerateToken(user.Username!, user.Role!, user.Id);

    return Ok(new AuthResponseDto
    {
        Success = true,
        Message = "Refresh token thành công",
        Token = newToken,
        UserId = user.Id,
        Username = user.Username,
        Role = user.Role
    });
}
    }
    

    // DTOs
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? GoogleId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
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

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}