using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CouponDTOs;

public class CouponForUpdatingDto
{
    public string Code { get; set; }

    [Required]
    [Range(1, 99, ErrorMessage = "DiscountPercentage must be >= 1% and <= 99%.")]
    public double DiscountPercentage { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "MaxDiscountAmount must be > 0.")]
    public double MaxDiscountAmount { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "MinPurchaseAmount must be > 0.")]
    public double MinPurchaseAmount { get; set; }

    public DateTime ExpirationDate { get; set; }
}