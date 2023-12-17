namespace Core.Entities.ProductAggregate;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> ImageUrls { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public int? BrandId { get; set; }
    public ProductBrand? Brand { get; set; }
    public int CategoryId { get; set; }
    public ProductCategory Category { get; set; }
    public List<ProductReview>? Reviews { get; set; }
    public double AverageRating
    {
        get
        {
            if (Reviews == null || Reviews.Count == 0)
                return 0;

            return Math.Round(Reviews.Average(r => r.Stars), 1);
        }
    } 
}