namespace API.Errors;

public class ErrorResponse
{
    public ErrorResponse(IEnumerable<string> errors, int statusCode = 400)
    {
        Errors = errors;
        StatusCode = statusCode;
    }

    public int StatusCode { get; init; }
    public IEnumerable<string> Errors { get; init; }
}
