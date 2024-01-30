namespace Core.DTOs.OrderDTOs;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public string ShippingAddress { get; set; }
    public string Status { get; set; }
    public DateTime OrderDate { get; set; }
    public double Subtotal { get; set; }
    public double Discount { get; set; }
    public double ShippingCosts { get; set; }
    public double Total { get; set; }
}
