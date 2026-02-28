using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("Articles")]
public class Article
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(255)]
    public string Slug { get; set; } = null!;

    [MaxLength(500)]
    public string? Summary { get; set; }

    public string? Content { get; set; }

    [MaxLength(500)]
    public string? Thumbnail { get; set; }

    public int CategoryId { get; set; }

    public bool IsPublished { get; set; } = false;

    public DateTime? PublishedAt { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual ArticleCategory Category { get; set; } = null!;
}