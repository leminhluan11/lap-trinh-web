using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("OrderStatusHistory")]
public class OrderStatusHistory
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int? PreviousStatus { get; set; }
    public int NewStatus { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    [MaxLength(100)]
    public string? UpdatedBy { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;
}