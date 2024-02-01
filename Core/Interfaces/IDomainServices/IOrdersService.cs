using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IOrdersService
{
    Task<PagedResult<OrderForAdminListDto>> GetOrders(OrderQueryParameters queryParams); //for admin view
    Task<PagedResult<OrderForCustomerListDto>> GetCustomerOrders(string customerEmail, PaginationQueryParameters queryParams); //for customer view
    Task<OrderDetailsDto> GetOrder(int id); //for admin view
    Task<OrderDetailsDto> GetCustomerOrder(string customerEmail, int orderId); //for customer view
    Task<int> CreateOrder(string customerEmail, OrderForCreatingDto orderDto);
    Task UpdateOrderStatus(int id, OrderStatusDto newStatusDto);
}