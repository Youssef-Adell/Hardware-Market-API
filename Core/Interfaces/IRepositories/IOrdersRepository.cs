using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IRepositories;

public interface IOrdersRepository
{
    void AddOrder(Order order);
}
