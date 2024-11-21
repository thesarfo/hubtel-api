namespace Hubtel.Api.Data.Response;


public class ApiResponse<T>
{
    public T? Content { get; set; }
    public ResponseMessage Message { get; set; } = new();

    public static ApiResponse<T> Success(T content, string? message = null)
    {
        return new ApiResponse<T>
        {
            Content = content,
            Message = new ResponseMessage
            {
                Type = "Success",
                Text = message ?? "Operation completed successfully."
            }
        };
    }

    public static ApiResponse<T> Failure(string errorMessage, string? details = null)
    {
        return new ApiResponse<T>
        {
            Content = default, 
            Message = new ResponseMessage
            {
                Type = "Error",
                Text = errorMessage
            }
        };
    }
}

public class ResponseMessage
{
    public string Type { get; set; } = "Info";
    public string Text { get; set; } = string.Empty;
}
