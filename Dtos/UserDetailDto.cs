namespace FashionEcommerce.DTOs;

public class UserDetailDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = null!;
    public bool IsLocked { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<UserAddressDto> Addresses { get; set; } = new List<UserAddressDto>();
}

public class UserAddressDto
{
    public int Id { get; set; }
    public string ContactName { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string AddressLine { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string District { get; set; } = null!;
    public string Ward { get; set; } = null!;
    public bool IsDefault { get; set; }
}