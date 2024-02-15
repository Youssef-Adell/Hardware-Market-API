using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.AuthDTOs;

public class ConfirmationEmailRequest
{
    [EmailAddress]
    public string Email { get; set; }
}
