namespace FashionEcommerce.DTOs;

public class CouponDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
    public string PromotionName { get; set; } = null!;
}