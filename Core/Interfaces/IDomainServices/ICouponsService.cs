using Core.DTOs.CouponDTOs;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IDomainServices;

public interface ICouponsService
{
    Task<IReadOnlyCollection<CouponDto>> GetCoupons();
    Task<CouponDto> GetCoupon(int id);
    Task<CouponDto> GetCoupon(string code);
    Task<int> AddCoupon(CouponForAddingDto couponToAdd);
    Task UpdateCoupon(int id, CouponForUpdatingDto updatedCoupon);
    Task DeleteCoupon(int id);
    Task<double> CalculateCouponDiscount(double subtotalAmount, string couponCode);
}