namespace Core.Entities.ProductAggregate;

public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string CustomerEmail { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}