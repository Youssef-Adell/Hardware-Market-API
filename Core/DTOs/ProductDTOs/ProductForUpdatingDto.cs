using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.ProductDTOs;

public class ProductForUpdatingDto
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Qunitiy must be > 0.")]
    public int Quantity { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Price must be > 0.")]
    public double Price { get; set; }

    [Required]
    public int BrandId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public List<int>? IdsOfImagesToRemove { get; set; }
}
