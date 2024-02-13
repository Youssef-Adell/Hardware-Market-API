using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IBrandsRepository
{
    Task<IReadOnlyCollection<Brand>> GetBrands();
    Task<Brand?> GetBrand(Guid id);
    Task<bool> BrandExists(Guid id);
    void AddBrand(Brand brand);
    void UpdateBrand(Brand brand);
    void DeleteBrand(Brand brand);
}
