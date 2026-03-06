namespace FashionEcommerce.DTOs;
public class GoogleLoginRequest
{
    public string GoogleId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;
}