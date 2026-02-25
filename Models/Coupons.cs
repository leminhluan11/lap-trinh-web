[Table("Coupons")]
public class Coupon
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public int UserId { get; set; }

    [Required]
    public int PromotionId { get; set; }

    public bool? IsUsed { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(PromotionId))]
    public virtual Promotion Promotion { get; set; } = null!;
}