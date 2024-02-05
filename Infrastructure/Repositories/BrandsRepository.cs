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

    public async Task<IReadOnlyCollection<ProductBrand>> GetBrands()
    {
        var brands = await appDbContext.ProductBrands
                            .AsNoTracking()
                            .ToListAsync();

        return brands;
    }

    public async Task<ProductBrand?> GetBrand(int id)
    {
        var brand = await appDbContext.ProductBrands
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id);

        return brand;
    }

    public async Task<bool> BrandExists(int id)
    {
        var exists = await appDbContext.ProductBrands.AnyAsync(brand => brand.Id == id);

        return exists;
    }

    public void AddBrand(ProductBrand brand)
    {
        appDbContext.ProductBrands.Add(brand);
    }

    public void UpdateBrand(ProductBrand brand)
    {
        appDbContext.ProductBrands.Update(brand);
    }

    public void DeleteBrand(ProductBrand brand)
    {
        appDbContext.ProductBrands.Remove(brand);
    }
}
