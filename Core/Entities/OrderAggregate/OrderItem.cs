namespace Core.Entities.OrderAggregate;

public class OrderItem : EntityBase
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImagePath { get; set; }
    public double Price { get; set; }
    public int Quntity { get; set; }
}