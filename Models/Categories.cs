using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("Categories")]
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Slug { get; set; } = null!;

    public int? ParentId { get; set; }

    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(ParentId))]
    public virtual Category? Parent { get; set; }

    public virtual ICollection<Category> Children { get; set; } = new List<Category>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}