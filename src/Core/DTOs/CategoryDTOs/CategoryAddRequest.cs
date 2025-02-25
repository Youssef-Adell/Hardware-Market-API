using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CategoryDTOs;

public class CategoryAddRequest
{
    [Required]
    public string Name { get; set; }
}
