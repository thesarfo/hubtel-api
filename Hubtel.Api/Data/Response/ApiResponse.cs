namespace Hubtel.Api.Data.Response;


public class ApiResponse<T>
{
    public T? Content { get; set; }
    public string Message { get; set; } = string.Empty;

    public static ApiResponse<T> Success(T content, string message)
    {
        return new ApiResponse<T>
        {
            Content = content,
            Message = message
        };
    }

    public static ApiResponse<T> Failure(string errorMessage, T? content = default)
    {
        return new ApiResponse<T>
        {
            Content = content,  
            Message = errorMessage  
        };
    }
}