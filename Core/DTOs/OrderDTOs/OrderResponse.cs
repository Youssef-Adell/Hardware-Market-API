using Core.Entities.OrderAggregate;

namespace Core.DTOs.OrderDTOs;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public List<OrderLineResponse> OrderLines { get; set; }
    public string ShippingAddress { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public double Subtotal { get; set; }
    public double Discount { get; set; }
    public double ShippingCosts { get; set; }
    public string PaymentIntentClientSecret { get; set; }
    public double Total { get; set; }
}

public class OrderLineResponse
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public int Quntity { get; set; }
}
