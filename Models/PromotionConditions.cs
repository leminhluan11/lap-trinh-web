[Table("PromotionConditions")]
public class PromotionCondition
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PromotionId { get; set; }

    [Required]
    [StringLength(50)]
    public string Field { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Operator { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Value { get; set; } = string.Empty;

    [ForeignKey(nameof(PromotionId))]
    public virtual Promotion Promotion { get; set; } = null!;
}