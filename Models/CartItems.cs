using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("CartItems")]
public class CartItem
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int ProductVariantId { get; set; }

    public int Quantity { get; set; } = 1;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(ProductVariantId))]
    public virtual ProductVariant Variant { get; set; } = null!;
}