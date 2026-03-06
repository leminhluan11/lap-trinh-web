using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FashionEcommerce.Data;
using FashionEcommerce.Models;
using FashionEcommerce.DTOs;  // Thêm using này để dùng DTOs

namespace FashionEcommerce.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly FashionEcommerceDbContext _context;

        public OrdersController(FashionEcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// POST /api/orders - Tạo đơn hàng mới (Client)
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<CreateOrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = new Order
                {
                    OrderCode = request.OrderCode,
                    ShippingName = request.ShippingName,
                    ShippingAddress = request.ShippingAddress,
                    ShippingPhone = request.ShippingPhone,
                    TotalAmount = request.TotalAmount,
                    DiscountAmount = request.DiscountAmount,
                    CouponCode = request.CouponCode,
                    ShippingFee = request.ShippingFee,
                    FinalAmount = request.FinalAmount,
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = request.PaymentStatus,
                    Status = request.Status,
                    OrderDate = DateTime.UtcNow
                };

                // Lấy userId từ token nếu có
                var userIdClaim = User.FindFirst("Id")?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    order.UserId = userId;
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Thêm chi tiết đơn hàng
                foreach (var item in request.OrderDetails)
                {
                    // Kiểm tra ProductVariantId có tồn tại không, nếu không thì tạo mới
                    var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId);
                    
                    int finalVariantId;
                    
                    // Nếu không tìm thấy variant, thử tìm theo ProductId
                    if (variant == null)
                    {
                        var productExists = await _context.Products.AnyAsync(p => p.Id == item.ProductVariantId);
                        if (productExists)
                        {
                            // Lấy color và size đầu tiên từ database
                            var defaultColor = await _context.MasterColors.FirstOrDefaultAsync();
                            var defaultSize = await _context.MasterSizes.FirstOrDefaultAsync();
                            
                            if (defaultColor == null || defaultSize == null)
                            {
                                return BadRequest(new CreateOrderResponse
                                {
                                    Success = false,
                                    Message = "Hệ thống chưa có thông tin màu sắc hoặc kích thước"
                                });
                            }

                            // Tạo variant tạm thời cho sản phẩm
                            variant = new ProductVariant
                            {
                                ProductId = item.ProductVariantId,
                                ColorId = defaultColor.Id,
                                SizeId = defaultSize.Id,
                                Sku = item.Sku,
                                Quantity = item.Quantity,
                                PriceModifier = 0
                            };
                            _context.ProductVariants.Add(variant);
                            await _context.SaveChangesAsync();
                            finalVariantId = variant.Id;
                        }
                        else
                        {
                            // Nếu product cũng không tồn tại, dùng variantId = 0
                            finalVariantId = 0;
                        }
                    }
                    else
                    {
                        finalVariantId = variant.Id;
                    }

                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductVariantId = finalVariantId,
                        Snapshot_ProductName = item.ProductName,
                        Snapshot_Sku = item.Sku,
                        Snapshot_Thumbnail = item.Thumbnail,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Color = item.Color,
                        Size = item.Size
                    };
                    _context.OrderDetails.Add(orderDetail);
                }

                await _context.SaveChangesAsync();

                return Ok(new CreateOrderResponse
                {
                    Success = true,
                    Message = "Đặt hàng thành công!",
                    OrderId = order.Id,
                    OrderCode = order.OrderCode
                });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new CreateOrderResponse
                {
                    Success = false,
                    Message = "Có lỗi xảy ra: " + innerMessage
                });
            }
        }

        /// <summary>
        /// GET /api/admin/orders - Danh sách đơn hàng
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderSummaryDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderCode = o.OrderCode,
                    OrderDate = o.OrderDate,
                    ShippingName = o.ShippingName,
                    ShippingAddress = o.ShippingAddress,
                    ShippingPhone = o.ShippingPhone,
                    TotalAmount = o.TotalAmount,
                    DiscountAmount = o.DiscountAmount,
                    ShippingFee = o.ShippingFee,
                    FinalAmount = o.FinalAmount,
                    PaymentMethod = o.PaymentMethod,
                    PaymentStatus = o.PaymentStatus,
                    Status = o.Status,
                    UserEmail = o.User != null ? o.User.Email : null
                })
                .ToListAsync();

            return Ok(orders);
        }

        /// <summary>
        /// GET /api/admin/orders/{id} - Chi tiết đơn hàng
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Details)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound(new { message = "Đơn hàng không tồn tại" });

            var dto = new OrderDetailDto
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                OrderDate = order.OrderDate,
                ShippingName = order.ShippingName,
                ShippingAddress = order.ShippingAddress,
                ShippingPhone = order.ShippingPhone,
                TotalAmount = order.TotalAmount,
                DiscountAmount = order.DiscountAmount,
                CouponCode = order.CouponCode,
                ShippingFee = order.ShippingFee,
                FinalAmount = order.FinalAmount,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                Status = order.Status,
                User = order.User != null ? new UserSummaryDto
                {
                    Id = order.User.Id,
                    Email = order.User.Email,
                    FullName = order.User.FullName,
                    PhoneNumber = order.User.PhoneNumber
                } : null,
                OrderDetails = order.Details.Select(d => new OrderItemDto
                {
                    Id = d.Id,
                    ProductVariantId = d.ProductVariantId,
                    Snapshot_ProductName = d.Snapshot_ProductName,
                    Snapshot_Sku = d.Snapshot_Sku,
                    Snapshot_Thumbnail = d.Snapshot_Thumbnail,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    Color = d.Color,
                    Size = d.Size
                }).ToList()
            };

            return Ok(dto);
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
        public async Task<ActionResult<OrderStatisticsDto>> GetOrderStatistics()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.Orders.SumAsync(o => o.FinalAmount);
            var totalDiscount = await _context.Orders.SumAsync(o => o.DiscountAmount);
            var totalShippingFee = await _context.Orders.SumAsync(o => o.ShippingFee);

            var ordersByStatus = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new OrderByStatusDto
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Amount = g.Sum(o => o.FinalAmount)
                })
                .ToListAsync();

            var ordersByPaymentStatus = await _context.Orders
                .GroupBy(o => o.PaymentStatus)
                .Select(g => new OrderByPaymentStatusDto
                {
                    PaymentStatus = g.Key ?? "Unknown",
                    Count = g.Count(),
                    Amount = g.Sum(o => o.FinalAmount)
                })
                .ToListAsync();

            var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            var statistics = new OrderStatisticsDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalDiscount = totalDiscount,
                TotalShippingFee = totalShippingFee,
                AverageOrderValue = averageOrderValue,
                OrdersByStatus = ordersByStatus,
                OrdersByPaymentStatus = ordersByPaymentStatus
            };

            return Ok(statistics);
        }
    }

    // Giữ nguyên request DTO cũ
    public class UpdateOrderStatusRequest
    {
        public int Status { get; set; }
        public string? Reason { get; set; }
    }
}