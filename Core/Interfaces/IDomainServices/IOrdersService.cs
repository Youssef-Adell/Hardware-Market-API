using Core.DTOs.OrderDTOs;
using Core.DTOs.SpecificationDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IOrdersService
{
    Task<int> CreateOrder(string customerEmail, OrderForCreatingDto orderDto);
    Task<PagedResult<OrderForAdminListDto>> GetOrders(OrdersSpecificationParameters specsParams);
    Task<OrderDetailsDto> GetOrder(int id);
}
