using Core.Entities.ProductAggregate;
using Core.Interfaces.IRepositories;

namespace Infrastructure.Repositories;

public class BrandsRepository : IBrandsRepository
{
    public Task<ProductBrand> GetBrand(int id)
    {
        throw new NotImplementedException();
    }

}
