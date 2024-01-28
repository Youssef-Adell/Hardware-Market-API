using Core.DTOs.OrderDTOs;

namespace Core.Interfaces.IDomainServices;

public interface IOrdersService
{
    Task<int> CreateOrder(string customerEmail, OrderForCreatingDto orderDto);
}
