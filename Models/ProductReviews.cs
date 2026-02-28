using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FashionEcommerce.Models;

[Table("ProductReviews")]
public class ProductReview
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int OrderId { get; set; }

    [Range(1, 5)]
    public int? Rating { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;
}