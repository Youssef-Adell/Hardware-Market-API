using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.OrderDTOs;

public class OrderAddRequest
{
    [Required]
    public IReadOnlyCollection<OrderLineAddRequest> OrderLines { get; set; }

    [Required]
    public ShippingAddressAddRequest ShippingAddress { get; set; }

    [Required]
    [RegularExpression(@"^01[0-9]\d{8}$", ErrorMessage = "Phone number should be in the form of 01X XXXX XXXX")]
    public string CustomerPhone { get; set; }

    public string? CouponCode { get; set; }
}

public class OrderLineAddRequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quntity cant be less than 1.")]
    public int Quntity { get; set; }
}

public class ShippingAddressAddRequest
{
    [Required]
    public string AddressLine { get; set; }

    [Required]
    public string City { get; set; }
}