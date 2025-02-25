using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CategoryDTOs;

public class CategoryUpdateRequest
{
    [Required]
    public string Name { get; set; }
}
