namespace FashionEcommerce.Models;

[Table("OrderDetails")]
public class OrderDetail
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }
    public int ProductVariantId { get; set; }

    [Required, MaxLength(200)]
    public string Snapshot_ProductName { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Snapshot_Sku { get; set; } = null!;

    [MaxLength(500)]
    public string? Snapshot_Thumbnail { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey(nameof(ProductVariantId))]
    public virtual ProductVariant Variant { get; set; } = null!;
}