using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionEcommerce.Data;
using FashionEcommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace FashionEcommerce.Controllers
{
    [ApiController]
    [Route("api/admin/promotions")]
    public class PromotionsController : ControllerBase
    {
        private readonly FashionEcommerceDbContext _context;

        public PromotionsController(FashionEcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET /api/admin/promotions - Danh sách chương trình khuyến mãi
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPromotions()
        {
            var promotions = await _context.Promotions
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.DiscountType,
                    p.DiscountValue,
                    p.StartDate,
                    p.EndDate,
                    p.IsActive,
                    p.Priority
                })
                .ToListAsync();

            return Ok(promotions);
        }

        /// <summary>
        /// POST /api/admin/promotions - Tạo chương trình khuyến mãi
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<object>> CreatePromotion([FromBody] CreatePromotionRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest(new { message = "Tên chương trình không được để trống" });

            if (request.DiscountType != "FIXED_AMOUNT" && request.DiscountType != "PERCENTAGE")
                return BadRequest(new { message = "DiscountType phải là FIXED_AMOUNT hoặc PERCENTAGE" });

            if (request.StartDate >= request.EndDate)
                return BadRequest(new { message = "Ngày kết thúc phải lớn hơn ngày bắt đầu" });

            var promotion = new Promotion
            {
                Name = request.Name,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive ?? true,
                Priority = request.Priority ?? 0
            };

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPromotionById), new { id = promotion.Id }, new
            {
                promotion.Id,
                promotion.Name,
                promotion.DiscountType,
                promotion.DiscountValue,
                promotion.IsActive
            });
        }

        /// <summary>
        /// GET /api/admin/promotions/{id} - Chi tiết chương trình khuyến mãi
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPromotionById(int id)
        {
            var promotion = await _context.Promotions
                .Include(p => p.Conditions)
                .Include(p => p.ProductPromotions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promotion == null)
                return NotFound(new { message = "Chương trình khuyến mãi không tồn tại" });

            return Ok(new
            {
                promotion.Id,
                promotion.Name,
                promotion.DiscountType,
                promotion.DiscountValue,
                promotion.StartDate,
                promotion.EndDate,
                promotion.IsActive,
                promotion.Priority,
                Conditions = promotion.Conditions,
                ProductPromotions = promotion.ProductPromotions
            });
        }

        /// <summary>
        /// PUT /api/admin/promotions/{id} - Cập nhật chương trình khuyến mãi
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePromotion(int id, [FromBody] UpdatePromotionRequest request)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
                return NotFound(new { message = "Chương trình khuyến mãi không tồn tại" });

            if (!string.IsNullOrEmpty(request.Name))
                promotion.Name = request.Name;

            if (!string.IsNullOrEmpty(request.DiscountType))
            {
                if (request.DiscountType != "FIXED_AMOUNT" && request.DiscountType != "PERCENTAGE")
                    return BadRequest(new { message = "DiscountType phải là FIXED_AMOUNT hoặc PERCENTAGE" });
                promotion.DiscountType = request.DiscountType;
            }

            if (request.DiscountValue.HasValue)
                promotion.DiscountValue = request.DiscountValue.Value;

            if (request.StartDate.HasValue)
                promotion.StartDate = request.StartDate.Value;

            if (request.EndDate.HasValue)
                promotion.EndDate = request.EndDate.Value;

            if (request.IsActive.HasValue)
                promotion.IsActive = request.IsActive.Value;

            if (request.Priority.HasValue)
                promotion.Priority = request.Priority.Value;

            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật chương trình khuyến mãi thành công", promotion.Id });
        }

        /// <summary>
        /// POST /api/admin/coupons/generate - Tạo voucher
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("coupons/generate")]
        public async Task<ActionResult<object>> GenerateCoupons([FromBody] GenerateCouponsRequest request)
        {
            // Kiểm tra Promotion có tồn tại
            var promotion = await _context.Promotions.FindAsync(request.PromotionId);
            if (promotion == null)
                return BadRequest(new { message = "Chương trình khuyến mãi không tồn tại" });

            // Kiểm tra User có tồn tại
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return BadRequest(new { message = "Người dùng không tồn tại" });

            // Tạo mã coupon
            var couponCode = GenerateCouponCode();

            // Kiểm tra mã đã tồn tại
            while (await _context.Coupons.AnyAsync(c => c.Code == couponCode))
            {
                couponCode = GenerateCouponCode();
            }

            var coupon = new Coupon
            {
                Code = couponCode,
                UserId = request.UserId,
                PromotionId = request.PromotionId,
                ExpiryDate = request.ExpiryDate ?? promotion.EndDate,
                IsUsed = false
            };

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo voucher thành công",
                coupon = new
                {
                    coupon.Id,
                    coupon.Code,
                    coupon.ExpiryDate,
                    PromotionName = promotion.Name
                }
            });
        }

        private string GenerateCouponCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }
    }

    public class CreatePromotionRequest
    {
        public string Name { get; set; } = null!;
        public string DiscountType { get; set; } = null!; // FIXED_AMOUNT hoặc PERCENTAGE
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
        public int? Priority { get; set; }
    }

    public class UpdatePromotionRequest
    {
        public string? Name { get; set; }
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public int? Priority { get; set; }
    }

    public class GenerateCouponsRequest
    {
        public int UserId { get; set; }
        public int PromotionId { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
