namespace FashionEcommerce.DTOs;

public class OrderSummaryDto
{
    public int Id { get; set; }
    public string OrderCode { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public string ShippingName { get; set; } = null!;
    public string ShippingAddress { get; set; } = null!;
    public string ShippingPhone { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal FinalAmount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string PaymentStatus { get; set; } = null!;
    public int Status { get; set; }
    public string? UserEmail { get; set; }  // Chỉ email, không lộ info user đầy đủ
}