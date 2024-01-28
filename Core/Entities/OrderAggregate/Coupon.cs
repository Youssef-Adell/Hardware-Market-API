namespace Core.Entities.OrderAggregate;

public class Coupon : EntityBase
{
    public string Code { get; set; }
    public double Value { get; set; }
}
