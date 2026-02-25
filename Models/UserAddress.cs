[Table("Articles")]
public class Article
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Summary { get; set; }

    public string? Content { get; set; }

    [StringLength(500)]
    public string? Thumbnail { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public bool? IsPublished { get; set; }

    public DateTime? PublishedAt { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual ArticleCategory Category { get; set; } = null!;
}