using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IBrandsRepository
{
    Task<ProductBrand> GetBrand(int id);
}
