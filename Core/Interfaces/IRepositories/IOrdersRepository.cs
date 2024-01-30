using Core.DTOs.SpecificationDTOs;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IRepositories;

public interface IOrdersRepository
{
    Task<PagedResult<Order>> GetOrders(OrdersSpecificationParameters specsParams);
    Task<PagedResult<Order>> GetCustomerOrders(string customerEmail, SpecificationParameters specsParams);
    Task<Order?> GetOrder(int id);
    void AddOrder(Order order);
}
