namespace Core.Entities.OrderAggregate;

public class Address
{
    public Address(string AddressLine, string City)
    {
        this.AddressLine = AddressLine;
        this.City = City;
    }

    public string AddressLine { get; set; }
    public string City { get; set; }
}