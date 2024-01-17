using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IBrandsRepository
{
    Task<IReadOnlyCollection<ProductBrand>> GetBrands();
    Task<ProductBrand?> GetBrand(int id);
}
