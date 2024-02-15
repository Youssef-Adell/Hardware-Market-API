namespace Core.Interfaces.IRepositories;

public interface IUnitOfWork
{
    IProductsRepository Products { get; }
    IProductReviewsRepository ProductReviews { get; }
    IBrandsRepository Brands { get; }
    ICategoriesRepository Categories { get; }
    ICouponsRepository Coupons { get; }
    IOrdersRepository Orders { get; }
    IUsersRepository Users { get; }
    Task SaveChanges();
}
