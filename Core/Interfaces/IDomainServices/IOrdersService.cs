using Core.DTOs.OrderDTOs;
using Core.DTOs.SpecificationDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IOrdersService
{
    Task<PagedResult<OrderForAdminListDto>> GetOrders(OrdersSpecificationParameters specsParams); //for admin view
    Task<OrderDetailsDto> GetOrder(int id); //for admin view
    Task<OrderDetailsDto> GetCustomerOrder(string customerEmail, int orderId); //for customer view
    Task<PagedResult<OrderForCustomerListDto>> GetCustomerOrders(string customerEmail, SpecificationParameters specsParams); //for customer view
    Task<int> CreateOrder(string customerEmail, OrderForCreatingDto orderDto);
}