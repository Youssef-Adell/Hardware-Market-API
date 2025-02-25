using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.ProductReviewDTOs;

public class ProductReviewAddRequest
{
    [Required]
    [Range(1, 5, ErrorMessage = "The Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [Required]
    [StringLength(maximumLength: 250, ErrorMessage = "The Comment must not exceed a maximum length of 250 character.")]
    public string Comment { get; set; }
}
