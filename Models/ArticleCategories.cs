[Table("ArticleCategories")]
public class ArticleCategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}