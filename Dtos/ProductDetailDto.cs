namespace FashionEcommerce.DTOs;

public class ProductDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Thumbnail { get; set; }
    public bool IsActive { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class ProductImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int SortOrder { get; set; }
}

public class ProductVariantDto
{
    public int Id { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    public string? Sku { get; set; }
    public int? Quantity { get; set; }
    public decimal? PriceModifier { get; set; }
    public string ColorHex {get;set;}
    public string SizeName {get;set;}
}