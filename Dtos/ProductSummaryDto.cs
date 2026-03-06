namespace FashionEcommerce.DTOs;

public class ProductSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Thumbnail { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public double Rating { get; set; }
public int ReviewCount { get; set; }
public List<ProductVariantDto>? Variants { get; set; }
}