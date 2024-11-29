namespace client.Shared.DTOs;

public class ErrorResponse
{
    public List<ApiError> Errors { get; set; } = new();
}

public class ApiError
{
    public string Code { get; set; }
    public string Message { get; set; }
}