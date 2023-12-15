namespace Apps.Customer.io.Models.Response;

public class ErrorResponse
{
    public IEnumerable<ErrorDetails>? Errors { get; set; }
    public ErrorMetadata? Meta { get; set; }
}