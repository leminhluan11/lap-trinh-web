using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("ProductVariants")]
public class ProductVariant
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }

    [Required, MaxLength(50)]
    public string Sku { get; set; } = null!;

    public int Quantity { get; set; } = 0;

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