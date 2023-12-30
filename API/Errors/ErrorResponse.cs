using System.Text.Json.Serialization;

namespace API.Errors;

public class ErrorResponse
{
    public ErrorResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    [JsonPropertyOrder(-2)]
    public int StatusCode { get; init; }
    [JsonPropertyOrder(-1)]
    public string Message { get; init; }
}
