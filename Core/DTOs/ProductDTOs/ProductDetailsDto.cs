namespace Core.DTOs.ProductDTOs;

public class ProductDetailsDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public List<string>? ImagesUrls { get; init; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; init; }
    public string Brand { get; init; }
    public string Category { get; init; }
    public bool InStock { get; init; }
    public float AverageRating { get; set; }
}
