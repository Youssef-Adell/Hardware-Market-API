using Core.DTOs.CouponDTOs;

namespace Core.Interfaces.IDomainServices;

public interface ICouponsService
{
    Task<IReadOnlyCollection<CouponResponse>> GetCoupons();
    Task<CouponResponse> GetCoupon(Guid id);
    Task<CouponResponse> GetCoupon(string code);
    Task<CouponResponse> AddCoupon(CouponAddRequest couponAddRequest);
    Task<CouponResponse> UpdateCoupon(Guid id, CouponUpdateRequest couponUpdateRequest);
    Task DeleteCoupon(Guid id);
    Task<double> CalculateCouponDiscount(double subtotalAmount, string couponCode);
}