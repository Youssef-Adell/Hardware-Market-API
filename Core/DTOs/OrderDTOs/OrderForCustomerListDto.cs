using Core.Entities.OrderAggregate;

namespace Core.DTOs.OrderDTOs;

public class OrderForCustomerListDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public double Total { get; set; }
}