using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IRepositories;

public interface IOrdersRepository
{
    Task<PagedResult<Order>> GetOrders(OrderQueryParameters queryParams);
    Task<PagedResult<Order>> GetCustomerOrders(Guid customerId, PaginationQueryParameters queryParams);
    Task<Order?> GetOrder(Guid id);
    void AddOrder(Order order);
    void UpdateOrder(Order order);
}
