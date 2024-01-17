using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CategoryDTOs;

public class CategoryForUpdatingDto
{
    [Required]
    public string Name { get; set; }
}
