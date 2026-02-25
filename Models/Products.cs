namespace FashionEcommerce.Models;

[Table("Products")]
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    [MaxLength(500)]
    public string? Thumbnail { get; set; }

    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public virtual ICollection<ProductPromotion> Promotions { get; set; } = new List<ProductPromotion>();
    public virtual ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}