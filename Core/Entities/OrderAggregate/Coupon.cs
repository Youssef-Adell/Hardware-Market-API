namespace Core.Entities.OrderAggregate;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; }
    public DiscountType Type { get; set; }
    public double Value { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsValid => ExpirationDate > DateTime.Now;
}