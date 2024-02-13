using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IDomainServices;

public interface IOrdersService
{
    Task<PagedResult<OrderForAdminListResponse>> GetOrders(OrderQueryParameters queryParams); //for admin view
    Task<PagedResult<OrderForCustomerListResponse>> GetCustomerOrders(string customerEmail, PaginationQueryParameters queryParams); //for customer view
    Task<OrderResponse> GetOrder(Guid id); //for admin view
    Task<OrderResponse> GetCustomerOrder(string customerEmail, Guid orderId); //for customer view
    Task<Guid> CreateOrder(string customerEmail, OrderAddRequest orderAddRequest);
    Task UpdateOrderStatus(Guid id, OrderStatus newOrderStatus);
}