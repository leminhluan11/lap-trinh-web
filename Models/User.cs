[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? Username { get; set; }

    [StringLength(255)]
    public string? PasswordHash { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    public string? GoogleId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [StringLength(20)]
    public string? Role { get; set; }

    public bool? IsLocked { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}