using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.SpecificationDTOs;

public class ProductsSpecificationParameters : SpecificationParameters, IValidatableObject
{
    public String? Search { get; set; }
    public uint MinPrice { get; set; }
    public uint MaxPrice { get; set; } = int.MaxValue;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MaxPrice <= MinPrice)
            yield return new ValidationResult("MaxPrice should be greater than MinPrice.");
    }

}
