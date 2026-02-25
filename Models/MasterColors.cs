namespace FashionEcommerce.Models;

[Table("MasterColors")]
public class MasterColor
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required, MaxLength(10)]
    public string HexCode { get; set; } = null!;

    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}