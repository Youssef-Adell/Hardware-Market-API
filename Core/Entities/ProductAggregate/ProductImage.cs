namespace Core.Entities.ProductAggregate;

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Path { get; set; }
}
