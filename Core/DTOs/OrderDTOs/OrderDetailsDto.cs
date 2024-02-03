using Core.Entities.OrderAggregate;

namespace Core.DTOs.OrderDTOs;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public string ShippingAddress { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public double Subtotal { get; set; }
    public double Discount { get; set; }
    public double ShippingCosts { get; set; }
    public string PaymentClientSecret { get; set; }
    public double Total { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public double Price { get; set; }
    public int Quntity { get; set; }
}
