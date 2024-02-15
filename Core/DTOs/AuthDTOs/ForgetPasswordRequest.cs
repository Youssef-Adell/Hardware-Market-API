using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.AuthDTOs;

public class ForgetPasswordRequest
{
    [EmailAddress]
    public string Email { get; set; }
}
