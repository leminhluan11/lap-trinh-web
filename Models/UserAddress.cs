namespace FashionEcommerce.Models;

[Table("UserAddresses")]
public class UserAddress
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required, MaxLength(100)]
    public string ContactName { get; set; } = null!;

    [Required, MaxLength(15)]
    public string ContactPhone { get; set; } = null!;

    [Required, MaxLength(255)]
    public string AddressLine { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Province { get; set; } = null!;

    [Required, MaxLength(50)]
    public string District { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Ward { get; set; } = null!;

    public bool IsDefault { get; set; } = false;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}