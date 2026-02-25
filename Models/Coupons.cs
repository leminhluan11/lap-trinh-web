namespace FashionEcommerce.Models;

[Table("Coupons")]
public class Coupon
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Code { get; set; } = null!;

    public int UserId { get; set; }
    public int PromotionId { get; set; }

    public bool IsUsed { get; set; } = false;

    public DateTime ExpiryDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(PromotionId))]
    public virtual Promotion Promotion { get; set; } = null!;
}