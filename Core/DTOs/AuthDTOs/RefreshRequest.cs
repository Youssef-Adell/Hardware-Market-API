namespace Core.DTOs.AuthDTOs;

public record RefreshRequest
{
    public string RefreshToken { get; init; }
}
