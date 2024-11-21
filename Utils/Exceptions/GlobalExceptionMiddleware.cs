using System.Text.Json;
using Hubtel.Api.Data.Response;

namespace Hubtel.Api.Utils.Exceptions;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); 
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        var errorResponse = ApiResponse<object>.Failure(
            errorMessage: exception.Message,
            details: exception.InnerException?.Message
        );

        var json = JsonSerializer.Serialize(errorResponse);
        return context.Response.WriteAsync(json);
    }

}
