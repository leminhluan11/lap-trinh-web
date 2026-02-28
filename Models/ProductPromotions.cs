using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("ProductPromotions")]
public class ProductPromotion
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }
    public int PromotionId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(PromotionId))]
    public virtual Promotion Promotion { get; set; } = null!;
}