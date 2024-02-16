using Core.Entities.UserAggregate;

namespace Core.Entities.ProductAggregate;

public class ProductReview
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid? CustomerId { get; set; }
    public User? Customer { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}