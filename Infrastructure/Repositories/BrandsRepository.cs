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

    public async Task<IReadOnlyCollection<Brand>> GetBrands()
    {
        var brands = await appDbContext.Brands
                            .AsNoTracking()
                            .ToListAsync();

        return brands;
    }

    public async Task<Brand?> GetBrand(Guid id)
    {
        var brand = await appDbContext.Brands
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id);

        return brand;
    }

    public async Task<bool> BrandExists(Guid id)
    {
        var exists = await appDbContext.Brands.AnyAsync(brand => brand.Id == id);

        return exists;
    }

    public void AddBrand(Brand brand)
    {
        appDbContext.Brands.Add(brand);
    }

    public void UpdateBrand(Brand brand)
    {
        appDbContext.Brands.Update(brand);
    }

    public void DeleteBrand(Brand brand)
    {
        appDbContext.Brands.Remove(brand);
    }
}
