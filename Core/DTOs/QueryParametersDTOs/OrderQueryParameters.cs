using Core.Entities.OrderAggregate;

namespace Core.DTOs.QueryParametersDTOs;

public class OrderQueryParameters : PaginationQueryParameters
{
    public string? Search { get; set; }
    public OrderStatus? Status { get; set; } //it accepts int and we need it to accept string (to be solved later)
}