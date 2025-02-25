using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext appDbContext;

    public UnitOfWork(AppDbContext appDbContext, IProductsRepository productsRepository, IProductReviewsRepository productReviewsRepository, IBrandsRepository brandsRepository, ICategoriesRepository categoriesRepository, ICouponsRepository couponsRepository, IOrdersRepository ordersRepository, IUsersRepository usersRepository)
    {
        this.appDbContext = appDbContext;
        Products = productsRepository;
        ProductReviews = productReviewsRepository;
        Brands = brandsRepository;
        Categories = categoriesRepository;
        Coupons = couponsRepository;
        Orders = ordersRepository;
        Users = usersRepository;
    }

    public IProductsRepository Products { get; private set; }
    public IProductReviewsRepository ProductReviews { get; private set; }
    public IBrandsRepository Brands { get; private set; }
    public ICategoriesRepository Categories { get; private set; }
    public ICouponsRepository Coupons { get; private set; }
    public IOrdersRepository Orders { get; private set; }
    public IUsersRepository Users { get; private set; }
    public async Task SaveChanges()
    {
        await appDbContext.SaveChangesAsync();
    }
}
