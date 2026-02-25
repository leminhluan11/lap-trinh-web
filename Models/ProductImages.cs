namespace FashionEcommerce.Models;

[Table("ProductImages")]
public class ProductImage
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [Required, MaxLength(500)]
    public string ImageUrl { get; set; } = null!;

    public int SortOrder { get; set; } = 0;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
}