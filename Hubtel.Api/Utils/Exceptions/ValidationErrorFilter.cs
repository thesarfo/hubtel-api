using Hubtel.Api.Data.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hubtel.Api.Utils.Exceptions;

public class ValidationErrorFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = ApiResponse<Dictionary<string, string[]>>.Failure(
                "Validation failed. Please correct the errors and try again.",
                errors
            );

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}