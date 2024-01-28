namespace Core.DTOs.ProductDTOs;

public class ProductForListDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string? ImageUrl { get; init; }
    public double Price { get; init; }
    public string Brand { get; init; }
    public string Category { get; init; }
    public int Quantity { get; set; }
    public float AverageRating { get; set; }
}
