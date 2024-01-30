using Core.DTOs.SpecificationDTOs;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IRepositories;

public interface IOrdersRepository
{
    void AddOrder(Order order);
    Task<PagedResult<Order>> GetOrders(OrdersSpecificationParameters specsParams);
    Task<Order?> GetOrder(int id);
}
