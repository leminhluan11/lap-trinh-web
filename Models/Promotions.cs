[Table("Promotions")]
public class Promotion
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string DiscountType { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public bool? IsActive { get; set; }

    public int? Priority { get; set; }

    public virtual ICollection<PromotionCondition> PromotionConditions { get; set; } = new List<PromotionCondition>();
    public virtual ICollection<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();
    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
}