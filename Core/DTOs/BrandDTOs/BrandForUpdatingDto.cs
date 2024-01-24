using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.BrandDTOs;

public class BrandForUpdatingDto
{
    [Required]
    public string Name { get; set; }
}
