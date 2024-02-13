namespace Core.DTOs.ProductDTOs;

public class ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<ProductImageResponse>? Images { get; init; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; init; }
    public string Brand { get; init; }
    public string Category { get; init; }
    public float AverageRating { get; set; }
}

public class ProductImageResponse
{
    public Guid Id { get; set; }
    public string Url { get; set; }
}