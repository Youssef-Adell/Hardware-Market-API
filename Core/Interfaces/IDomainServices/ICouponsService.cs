using Core.DTOs.CouponDTOs;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IDomainServices;

public interface ICouponsService
{
    Task<IReadOnlyCollection<CouponResponse>> GetCoupons();
    Task<CouponResponse> GetCoupon(Guid id);
    Task<CouponResponse> GetCoupon(string code);
    Task<Guid> AddCoupon(CouponAddRequest couponAddRequest);
    Task UpdateCoupon(Guid id, CouponUpdateRequest couponUpdateRequest);
    Task DeleteCoupon(Guid id);
    Task<double> CalculateCouponDiscount(double subtotalAmount, string couponCode);
}