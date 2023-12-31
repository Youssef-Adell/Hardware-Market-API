namespace API.Errors;

public class ValidationErrorResponse : ErrorResponse
{

    public ValidationErrorResponse(IEnumerable<string> details) : base(400, "One or more validation errors occurred.")
    {
        Details = details;
    }

    public IEnumerable<string> Details { get; init; }

}
