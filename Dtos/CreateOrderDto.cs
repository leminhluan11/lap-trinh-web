namespace FashionEcommerce.DTOs;

public class CreateOrderRequest
{
    public string OrderCode { get; set; } = null!;
    public string ShippingName { get; set; } = null!;
    public string ShippingAddress { get; set; } = null!;
    public string ShippingPhone { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? CouponCode { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal FinalAmount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = "Unpaid";
    public int Status { get; set; } = 0;
    public List<CreateOrderDetailRequest> OrderDetails { get; set; } = new List<CreateOrderDetailRequest>();
}

public class CreateOrderDetailRequest
{
    public int ProductVariantId { get; set; }  // Có thể là ProductId từ client
    public string ProductName { get; set; } = null!;
    public string Sku { get; set; } = null!;
    public string? Thumbnail { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreateOrderResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public int? OrderId { get; set; }
    public string? OrderCode { get; set; }
}
