using System.ComponentModel.DataAnnotations;
using Core.Entities.OrderAggregate;

namespace Core.DTOs.OrderDTOs;

public class OrderStatusUpdateRequest
{
    [Required]
    public OrderStatus Status { get; set; }
}
