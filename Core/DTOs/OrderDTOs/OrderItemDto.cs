namespace Core.DTOs.OrderDTOs;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public double Price { get; set; }
    public int Quntity { get; set; }
}
