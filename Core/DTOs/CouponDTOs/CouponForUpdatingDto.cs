using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CouponDTOs;

public class CouponForUpdatingDto : IValidatableObject
{
    public string Code { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "DiscountPercentage must be > 0.")]
    public double DiscountPercentage { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "MaxDiscountAmount must be > 0.")]
    public double MaxDiscountAmount { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "MinPurchaseAmount must be > 0.")]
    public double MinPurchaseAmount { get; set; }

    public DateTime ExpirationDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPurchaseAmount <= MaxDiscountAmount)
            yield return new ValidationResult("MinPurchaseAmount must be > MaxDiscountAmount.");
    }

}