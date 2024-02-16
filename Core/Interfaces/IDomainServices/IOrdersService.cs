using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces.IDomainServices;

public interface IOrdersService
{
    Task<PagedResult<OrderForAdminListResponse>> GetOrders(OrderQueryParameters queryParams); //for admin view
    Task<PagedResult<OrderForCustomerListResponse>> GetCustomerOrders(Guid customerId, PaginationQueryParameters queryParams); //for customer view
    Task<OrderResponse> GetOrder(Guid id); //for admin view
    Task<OrderResponse> GetCustomerOrder(Guid customerId, Guid orderId); //for customer view
    Task<OrderResponse> CreateOrder(Guid customerId, OrderAddRequest orderAddRequest);
    Task<OrderResponse> UpdateOrderStatus(Guid id, OrderStatus newOrderStatus);
}