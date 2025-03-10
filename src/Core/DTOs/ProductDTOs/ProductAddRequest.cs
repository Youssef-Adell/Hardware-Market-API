using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.ProductDTOs;

public class ProductAddRequest
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
    public Guid BrandId { get; set; }

    [Required]
    public Guid CategoryId { get; set; }
}
