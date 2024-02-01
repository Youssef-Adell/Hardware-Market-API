namespace Core.DTOs.QueryParametersDTOs;

public class ReviewQueryParameters : PaginationQueryParameters
{
    public string? CustomerEmail { get; set; }
}
