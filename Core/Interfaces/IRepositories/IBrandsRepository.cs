using Core.Entities.ProductAggregate;

namespace Core.Interfaces.IRepositories;

public interface IBrandsRepository
{
    Task<IReadOnlyCollection<ProductBrand>> GetBrands();
    Task<ProductBrand?> GetBrand(int id);
    void AddBrand(ProductBrand brand);
    void UpdateBrand(ProductBrand brand);
    void DeleteBrand(ProductBrand brand);
    Task SaveChanges();
}
