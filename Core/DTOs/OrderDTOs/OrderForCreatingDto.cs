using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.OrderDTOs;

public class OrderForCreatingDto
{
    [Required]
    public IReadOnlyCollection<OrderItemForCreatingDto> OrderItems { get; set; }

    [Required]
    public AddressDto ShippingAddress { get; set; }

    [Required]
    [RegularExpression(@"^01[0-9]\d{8}$", ErrorMessage = "Phone number should be in the form of 01X XXXX XXXX")]
    public string CustomerPhone { get; set; }

    public string? CouponCode { get; set; }
}