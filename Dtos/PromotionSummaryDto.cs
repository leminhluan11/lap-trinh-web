namespace FashionEcommerce.DTOs;

public class PromotionSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public int Priority { get; set; }
}