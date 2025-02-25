namespace Core.DTOs.AuthDTOs;

public record LoginResponse
{
    public Guid UserId { get; init; }
    public string UserName { get; init; }
    public string UserEmail { get; init; }
    public string UserRole { get; init; }
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}