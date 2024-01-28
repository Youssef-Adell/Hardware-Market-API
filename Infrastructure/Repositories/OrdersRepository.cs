using Core.Entities.OrderAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;

namespace Infrastructure.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly AppDbContext appDbContext;

    public OrdersRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public void AddOrder(Order order)
    {
        appDbContext.Orders.Add(order);
    }

    public async Task SaveChanges()
    {
        await appDbContext.SaveChangesAsync();
    }

}
