namespace FashionEcommerce.Models;

[Table("PromotionConditions")]
public class PromotionCondition
{
    [Key]
    public int Id { get; set; }

    public int PromotionId { get; set; }

    [Required, MaxLength(50)]
    public string Field { get; set; } = null!;

    [Required, MaxLength(20)]
    public string Operator { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Value { get; set; } = null!;

    [ForeignKey(nameof(PromotionId))]
    public virtual Promotion Promotion { get; set; } = null!;
}