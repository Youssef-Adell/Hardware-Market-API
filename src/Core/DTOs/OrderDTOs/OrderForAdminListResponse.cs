using Core.Entities.OrderAggregate;

namespace Core.DTOs.OrderDTOs;

public class OrderForAdminListResponse
{
    public Guid Id { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string City { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public double Total { get; set; }
}