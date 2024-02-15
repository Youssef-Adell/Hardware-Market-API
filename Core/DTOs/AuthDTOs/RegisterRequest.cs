using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.AuthDTOs;

public record RegisterRequest
{
    [Required]
    public string UserName { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    public string Password { get; init; }
}