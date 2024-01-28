using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IRepositories;

public interface ICouponsRepository
{
    Task<Coupon?> GetCoupon(string code);
}
