namespace Core.DTOs.SpecificationDTOs;

public class ProductsSpecificationParameters : SpecificationParameters
{
    public String? Search { get; set; }
    public uint MinPrice { get; set; }
    public uint MaxPrice { get; set; } = int.MaxValue;
}
