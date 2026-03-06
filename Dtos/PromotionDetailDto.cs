namespace FashionEcommerce.DTOs;

public class PromotionDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DiscountType { get; set; } = null!;
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public List<PromotionConditionDto> Conditions { get; set; } = new List<PromotionConditionDto>();
    public List<ProductPromotionDto> ProductPromotions { get; set; } = new List<ProductPromotionDto>();
}

public class PromotionConditionDto
{
    public int Id { get; set; }
    public string Field { get; set; } = null!;
    public string Operator { get; set; } = null!;
    public string Value { get; set; } = null!;
}

public class ProductPromotionDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int PromotionId { get; set; }
}