using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("MasterSizes")]
public class MasterSize
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string Name { get; set; } = null!;

    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}