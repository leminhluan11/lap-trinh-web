using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FashionEcommerce.Models;

[Table("Notifications")]
public class Notification
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(500)]
    public string Message { get; set; } = null!;

    public bool IsRead { get; set; } = false;

    [MaxLength(50)]
    public string? Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}