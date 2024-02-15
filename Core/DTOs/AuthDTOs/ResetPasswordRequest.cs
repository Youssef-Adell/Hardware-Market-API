using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.AuthDTOs;

public class ResetPasswordRequest
{
    public string AccountId { get; set; }

    public string Token { get; set; }

    [MinLength(8, ErrorMessage = "The password length should not be less than 8.")]
    public string NewPassword { get; set; }
}