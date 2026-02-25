[Table("ProductReviews")]
public class ProductReview
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;
}