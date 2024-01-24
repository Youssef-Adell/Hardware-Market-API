using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.BrandDTOs;

public class BrandForAddingDto
{
    [Required]
    public string Name { get; set; }
}
