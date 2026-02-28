using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("Promotions")]
public class Promotion
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(20)]
    public string DiscountType { get; set; } = null!; // FIXED_AMOUNT hoặc PERCENTAGE

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 0;

    public virtual ICollection<PromotionCondition> Conditions { get; set; } = new List<PromotionCondition>();
    public virtual ICollection<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();
    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
}