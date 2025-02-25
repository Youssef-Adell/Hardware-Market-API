using Core.Entities.OrderAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CouponsRepository : ICouponsRepository
{
    private readonly AppDbContext appDbContext;

    public CouponsRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<IReadOnlyCollection<Coupon>> GetCoupons()
    {
        var coupons = await appDbContext.Coupons
                            .AsNoTracking()
                            .ToListAsync();

        return coupons;
    }

    public async Task<Coupon?> GetCoupon(Guid id)
    {
        var coupon = await appDbContext.Coupons
                            .AsNoTracking()
                            .FirstOrDefaultAsync(c => c.Id == id);

        return coupon;
    }

    public async Task<Coupon?> GetCoupon(string code)
    {
        var coupon = await appDbContext.Coupons
                            .AsNoTracking()
                            .FirstOrDefaultAsync(c => c.Code.ToLower() == code.ToLower());

        return coupon;
    }

    public async Task<bool> CouponExists(string code)
    {
        var exists = await appDbContext.Coupons.AnyAsync(coupon => coupon.Code == code);

        return exists;
    }

    public void AddCoupon(Coupon Coupon)
    {
        appDbContext.Coupons.Add(Coupon);
    }

    public void UpdateCoupon(Coupon Coupon)
    {
        appDbContext.Coupons.Update(Coupon);
    }

    public void DeleteCoupon(Coupon Coupon)
    {
        appDbContext.Coupons.Remove(Coupon);
    }
}
