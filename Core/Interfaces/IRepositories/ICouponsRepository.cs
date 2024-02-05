using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IRepositories;

public interface ICouponsRepository
{
    Task<IReadOnlyCollection<Coupon>> GetCoupons();
    Task<Coupon?> GetCoupon(int id);
    Task<Coupon?> GetCoupon(string code);
    Task<bool> CouponExists(string code);
    void AddCoupon(Coupon Coupon);
    void UpdateCoupon(Coupon Coupon);
    void DeleteCoupon(Coupon Coupon);
}
