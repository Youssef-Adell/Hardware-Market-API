using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.QueryParametersDTOs;

public class ProductQueryParameters : PaginationQueryParameters, IValidatableObject
{
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
    public String? Search { get; set; }
    public uint MinPrice { get; set; } = 0;
    public uint MaxPrice { get; set; } = int.MaxValue;
    public int? CategoryId { get; set; } = null;
    public IEnumerable<int>? BrandId { get; set; } = null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MaxPrice <= MinPrice)
            yield return new ValidationResult("MaxPrice should be greater than MinPrice.");
    }

}
