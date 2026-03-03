namespace FashionEcommerce.DTOs;

public class UserSummaryDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = null!;
    public bool IsLocked { get; set; }
    public DateTime CreatedAt { get; set; }
}