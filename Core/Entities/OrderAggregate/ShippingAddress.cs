namespace Core.Entities.OrderAggregate;

public class ShippingAddress
{
    public ShippingAddress(string AddressLine, string City)
    {
        this.AddressLine = AddressLine;
        this.City = City;
    }

    public string AddressLine { get; set; }
    public string City { get; set; }

    public override string ToString()
    {
        return $"{AddressLine}, {City}";
    }
}