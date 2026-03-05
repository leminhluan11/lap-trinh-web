using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionEcommerce.Data;
using FashionEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using FashionEcommerce.DTOs;  // Thêm using này để dùng DTO

namespace FashionEcommerce.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly FashionEcommerceDbContext _context;

        public UsersController(FashionEcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET /api/admin/users - Danh sách người dùng
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSummaryDto>>> GetUsers()
        {
            var users = await _context.Users.Select(u => new UserSummaryDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Username = u.Username,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                IsLocked = u.IsLocked,
                CreatedAt = u.CreatedAt
            }).ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// GET /api/admin/users/{id} - Chi tiết người dùng
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailDto>> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Người dùng không tồn tại" });

            var dto = new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                IsLocked = user.IsLocked,
                CreatedAt = user.CreatedAt,
                Addresses = user.Addresses.Select(a => new UserAddressDto
                {
                    Id = a.Id,
                    ContactName = a.ContactName,
                    ContactPhone = a.ContactPhone,
                    AddressLine = a.AddressLine,
                    Province = a.Province,
                    District = a.District,
                    Ward = a.Ward,
                    IsDefault = a.IsDefault
                }).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// PUT /api/admin/users/{id}/lock - Khóa tài khoản
        /// </summary>
        [HttpPut("{id}/lock")]
        public async Task<ActionResult> LockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Người dùng không tồn tại" });

            user.IsLocked = !user.IsLocked;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                message = user.IsLocked ? "Khóa tài khoản thành công" : "Mở khóa tài khoản thành công", 
                isLocked = user.IsLocked 
            });
        }

        /// <summary>
        /// PUT /api/admin/users/{id}/role - Đổi quyền
        /// </summary>
        [HttpPut("{id}/role")]
        public async Task<ActionResult> UpdateUserRole(int id, [FromBody] UpdateRoleRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Người dùng không tồn tại" });

            if (string.IsNullOrEmpty(request.Role))
                return BadRequest(new { message = "Role không được để trống" });

            user.Role = request.Role;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật quyền thành công", role = user.Role });
        }
    }

    // Giữ nguyên request DTO cũ của bạn
    public class UpdateRoleRequest
    {
        public string Role { get; set; } = null!;
    }
}