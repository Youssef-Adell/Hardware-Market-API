using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CouponDTOs;

public class CouponForAddingDto : IValidatableObject
{
    [Required]
    public string Code { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "DiscountPercentage must be > 0.")]
    public double DiscountPercentage { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "MaxDiscountAmount must be > 0.")]
    public double MaxDiscountAmount { get; set; }

    [Required]
    public double MinPurchaseAmount { get; set; }

    [Required]
    public DateTime ExpirationDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPurchaseAmount <= MaxDiscountAmount)
            yield return new ValidationResult("MinPurchaseAmount must be > MaxDiscountAmount.");
    }

}
