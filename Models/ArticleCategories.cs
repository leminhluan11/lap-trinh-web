using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("ArticleCategories")]
public class ArticleCategory
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Slug { get; set; } = null!;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}