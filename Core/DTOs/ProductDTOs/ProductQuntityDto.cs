using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.ProductDTOs;

public class ProductQuntityDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Qunitiy must be greater than 0.")]
    public int Quantity { get; set; }
}
