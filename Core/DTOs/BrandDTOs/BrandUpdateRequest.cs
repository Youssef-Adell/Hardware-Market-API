using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.BrandDTOs;

public class BrandUpdateRequest
{
    [Required]
    public string Name { get; set; }
}
