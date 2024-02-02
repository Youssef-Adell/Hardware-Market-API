using Core.Entities.OrderAggregate;

namespace Core.DTOs.CouponDTOs;

public class CouponForUpdatingDto
{
    public string Code { get; set; }
    public DiscountType Type { get; set; }
    public double Value { get; set; }
    public DateTime ExpirationDate { get; set; }
}
