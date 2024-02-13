using Core.Entities.OrderAggregate;

namespace Core.DTOs.OrderDTOs;

public class OrderForCustomerListResponse
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public double Total { get; set; }
}