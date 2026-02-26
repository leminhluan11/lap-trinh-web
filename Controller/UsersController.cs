using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionEcommerce.Data;
using FashionEcommerce.Models;

namespace FashionEcommerce.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
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
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.Users.Select(u => new
            {
                u.Id,
                u.Email,
                u.FullName,
                u.Username,
                u.PhoneNumber,
                u.Role,
                u.IsLocked,
                u.CreatedAt
            }).ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// GET /api/admin/users/{id} - Chi tiết người dùng
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Người dùng không tồn tại" });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Username,
                user.PhoneNumber,
                user.DateOfBirth,
                user.AvatarUrl,
                user.Role,
                user.IsLocked,
                user.CreatedAt,
                Addresses = user.Addresses
            });
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

            return Ok(new { message = user.IsLocked ? "Khóa tài khoản thành công" : "Mở khóa tài khoản thành công", user.IsLocked });
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

            return Ok(new { message = "Cập nhật quyền thành công", user.Role });
        }
    }

    public class UpdateRoleRequest
    {
        public string Role { get; set; } = null!;
    }
}
