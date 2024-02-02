namespace Core.Entities.OrderAggregate;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; }
    public double DiscountPercentage { get; set; } //ex: 10 means 10% discount
    public double MaxDiscountAmount { get; set; } //To limit the discount
    public double MinPurchaseAmount { get; set; } //To make coupon can be applied only for orders with subtotal >= this value
    public DateTime ExpirationDate { get; set; }
    public bool IsValid => ExpirationDate > DateTime.Now;
}