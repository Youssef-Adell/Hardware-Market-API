using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.OrderDTOs;

public class AddressDto
{
    [Required]
    public string AddressLine { get; set; }

    [Required]
    public string City { get; set; }
}
