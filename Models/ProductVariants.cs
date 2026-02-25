[Table("ProductVariants")]
public class ProductVariant
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int ColorId { get; set; }

    [Required]
    public int SizeId { get; set; }

    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = string.Empty;

    public int? Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? PriceModifier { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(ColorId))]
    public virtual MasterColor Color { get; set; } = null!;

    [ForeignKey(nameof(SizeId))]
    public virtual MasterSize Size { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}