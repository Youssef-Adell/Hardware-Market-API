using Core.Entities.ProductAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BrandsRepository : IBrandsRepository
{
    private readonly AppDbContext appDbContext;

    public BrandsRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<ProductBrand?> GetBrand(int id)
    {
        var brand = await appDbContext.ProductBrands
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id);

        return brand;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetBrands()
    {
        var brands = await appDbContext.ProductBrands
                        .AsNoTracking()
                        .ToListAsync();

        return brands;
    }

}
