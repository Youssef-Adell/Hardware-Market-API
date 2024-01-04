namespace Core.Entities.ProductAggregate;

public class ProductReview : EntityBase
{
    public int ProductId { get; set; }
    public string CustomerEmail { get; set; }
    public int Stars { get; set; }
    public string Comment { get; set; }
}