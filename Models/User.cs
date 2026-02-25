using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionEcommerce.Models;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? Username { get; set; }

    [MaxLength(255)]
    public string? PasswordHash { get; set; }

    [Required, MaxLength(100), EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(100)]
    public string? GoogleId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = null!;

    [MaxLength(15)]
    public string? PhoneNumber { get; set; }

    [MaxLength(500)]
    public string? AvatarUrl { get; set; }

    [MaxLength(20)]
    public string Role { get; set; } = "Customer";

    public bool IsLocked { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}