namespace Core.Entities.OrderAggregate;

public class Order : EntityBase
{
    public List<OrderItem> OrderItems { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public Address ShippingAddress { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public double Subtotal { get; set; }
    public double Discount { get; set; }
    public double ShippingCosts { get; set; }
    public double Total => (Subtotal + ShippingCosts) - Discount;
}