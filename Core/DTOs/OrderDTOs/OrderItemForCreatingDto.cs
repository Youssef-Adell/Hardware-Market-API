using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.OrderDTOs;

public class OrderItemForCreatingDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quntity cant be less than 1.")]
    public int Quntity { get; set; }
}