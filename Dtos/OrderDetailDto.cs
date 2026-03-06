namespace FashionEcommerce.DTOs;

public class OrderDetailDto
{
    public int Id { get; set; }
    public string OrderCode { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public string ShippingName { get; set; } = null!;
    public string ShippingAddress { get; set; } = null!;
    public string ShippingPhone { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? CouponCode { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal FinalAmount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public int Status { get; set; }
    public UserSummaryDto? User { get; set; }  // DTO con cho user
    public List<OrderItemDto> OrderDetails { get; set; } = new List<OrderItemDto>();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductVariantId { get; set; }
    public string Snapshot_ProductName { get; set; } = null!;
    public string Snapshot_Sku { get; set; } = null!;
    public string? Snapshot_Thumbnail { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
}