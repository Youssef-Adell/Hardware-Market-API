namespace Core.DTOs.ProductReviewDTOs;

public class ProductReviewDto
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}