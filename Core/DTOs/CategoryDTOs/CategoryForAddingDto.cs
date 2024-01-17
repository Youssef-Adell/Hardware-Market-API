using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CategoryDTOs;

public class CategoryForAddingDto
{
    [Required]
    public string Name { get; set; }
}
