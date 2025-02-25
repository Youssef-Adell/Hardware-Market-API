namespace Core.DTOs.CouponDTOs;

public class CouponResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public double DiscountPercentage { get; set; }
    public double MaxDiscountAmount { get; set; }
    public double MinPurchaseAmount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsValid { get; set; }
}