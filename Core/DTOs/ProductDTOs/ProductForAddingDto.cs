namespace Core.DTOs.ProductDTOs;

public class ProductForAddingDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public int? BrandId { get; set; }
    public int CategoryId { get; set; }
}
