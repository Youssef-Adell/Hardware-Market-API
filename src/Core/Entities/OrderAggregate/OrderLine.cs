namespace Core.Entities.OrderAggregate;

public class OrderLine
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public int Quntity { get; set; }
}