namespace Core.Entities.ProductAggregate;

public class Product : EntityBase
{
    public string Name { get; set; }
    public List<string>? ImageUrls { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public int BrandId { get; set; }
    public ProductBrand Brand { get; set; }
    public int CategoryId { get; set; }
    public ProductCategory Category { get; set; }
    public List<ProductReview>? Reviews { get; set; }
    public float AverageRating { get; set; }
}