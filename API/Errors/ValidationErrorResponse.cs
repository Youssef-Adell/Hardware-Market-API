namespace API.Errors;

public class ValidationErrorResponse : ErrorResponse
{

    public ValidationErrorResponse(IEnumerable<string> errors) : base(400, "One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public IEnumerable<string> Errors { get; init; }

}
