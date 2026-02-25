[Table("OrderDetails")]
public class OrderDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public int ProductVariantId { get; set; }

    [Required]
    [StringLength(200)]
    public string Snapshot_ProductName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Snapshot_Sku { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Snapshot_Thumbnail { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey(nameof(ProductVariantId))]
    public virtual ProductVariant ProductVariant { get; set; } = null!;
}