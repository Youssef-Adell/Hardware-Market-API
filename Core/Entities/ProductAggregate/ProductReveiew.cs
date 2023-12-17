namespace Core.Entities.ProductAggregate;

public class ProductReview
{
    public int Id { get; set; }
    public int ProductId {get; set;}
    public string CustomerEmail { get; set; }
    public int Stars { get; set; }
    public string Comment { get; set; }
}