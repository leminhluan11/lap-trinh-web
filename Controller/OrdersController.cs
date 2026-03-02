using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FashionEcommerce.Data;
using FashionEcommerce.Models;

namespace FashionEcommerce.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly FashionEcommerceDbContext _context;

        public OrdersController(FashionEcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET /api/admin/orders - Danh sách đơn hàng
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Select(o => new
                {
                    o.Id,
                    o.OrderCode,
                    o.OrderDate,
                    o.ShippingName,
                    o.ShippingAddress,
                    o.ShippingPhone,
                    o.TotalAmount,
                    o.DiscountAmount,
                    o.ShippingFee,
                    o.FinalAmount,
                    o.PaymentMethod,
                    o.PaymentStatus,
                    o.Status,
                    UserEmail = o.User != null ? o.User.Email : null
                })
                .ToListAsync();

            return Ok(orders);
        }

        /// <summary>
        /// GET /api/admin/orders/{id} - Chi tiết đơn hàng
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Details)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound(new { message = "Đơn hàng không tồn tại" });

            return Ok(new
            {
                order.Id,
                order.OrderCode,
                order.OrderDate,
                order.ShippingName,
                order.ShippingAddress,
                order.ShippingPhone,
                order.TotalAmount,
                order.DiscountAmount,
                order.CouponCode,
                order.ShippingFee,
                order.FinalAmount,
                order.PaymentMethod,
                order.PaymentStatus,
                order.Status,
                User = order.User != null ? new
                {
                    order.User.Id,
                    order.User.Email,
                    order.User.FullName,
                    order.User.PhoneNumber
                } : null,
                OrderDetails = order.Details
            });
        }

        /// <summary>
        /// PUT /api/admin/orders/{id}/status - Cập nhật trạng thái
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound(new { message = "Đơn hàng không tồn tại" });

            if (request.Status < 0 || request.Status > 5)
                return BadRequest(new { message = "Trạng thái không hợp lệ" });

            int oldStatus = order.Status;
            order.Status = request.Status;
            _context.Orders.Update(order);

            // Thêm vào lịch sử thay đổi trạng thái
            var statusHistory = new OrderStatusHistory
            {
                OrderId = id,
                PreviousStatus = oldStatus,
                NewStatus = request.Status,
                Timestamp = DateTime.UtcNow,
                Note = request.Reason
            };
            _context.OrderStatusHistories.Add(statusHistory);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật trạng thái thành công", order.Status });
        }

        /// <summary>
        /// GET /api/admin/orders/statistics - Thống kê
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetOrderStatistics()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.Orders.SumAsync(o => o.FinalAmount);
            var totalDiscount = await _context.Orders.SumAsync(o => o.DiscountAmount);
            var totalShippingFee = await _context.Orders.SumAsync(o => o.ShippingFee);

            var ordersByStatus = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Amount = g.Sum(o => o.FinalAmount)
                })
                .ToListAsync();

            var ordersByPaymentStatus = await _context.Orders
                .GroupBy(o => o.PaymentStatus)
                .Select(g => new
                {
                    PaymentStatus = g.Key,
                    Count = g.Count(),
                    Amount = g.Sum(o => o.FinalAmount)
                })
                .ToListAsync();

            var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            return Ok(new
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalDiscount = totalDiscount,
                TotalShippingFee = totalShippingFee,
                AverageOrderValue = averageOrderValue,
                OrdersByStatus = ordersByStatus,
                OrdersByPaymentStatus = ordersByPaymentStatus
            });
        }
    }

    public class UpdateOrderStatusRequest
    {
        public int Status { get; set; }
        public string? Reason { get; set; }
    }
}
