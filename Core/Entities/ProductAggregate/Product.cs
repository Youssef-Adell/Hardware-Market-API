namespace Core.Entities.ProductAggregate;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<ProductImage> Images { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid BrandId { get; set; }
    public Brand? Brand { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public List<ProductReview>? Reviews { get; set; }
    public double AverageRating { get; set; }
}