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

    public async Task<Coupon?> GetCoupon(string code)
    {
        var coupon = await appDbContext.Coupons
                            .AsNoTracking()
                            .FirstOrDefaultAsync(c => c.Code == code);

        return coupon;
    }

}
