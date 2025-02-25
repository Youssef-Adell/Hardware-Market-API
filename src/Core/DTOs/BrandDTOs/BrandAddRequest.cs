using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.BrandDTOs;

public class BrandAddRequest
{
    [Required]
    public string Name { get; set; }
}
