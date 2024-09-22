using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Caesar.API.Middleware;

public class GlobalExceptionHandler : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        HttpStatusCode statusCode;
        string message;

        var exceptionType = context.Exception.GetType();
        if (exceptionType == typeof(UnauthorizedAccessException))
        {
            statusCode = HttpStatusCode.Unauthorized;
            message = "Unauthorized access";
        }
        else if (exceptionType == typeof(KeyNotFoundException))
        {
            statusCode = HttpStatusCode.NotFound;
            message = "Requested resource not found";
        }
        else
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = "An unexpected error occurred";
        }

        _logger.LogError(context.Exception, message);

        context.Result = new ObjectResult(new { error = message })
        {
            StatusCode = (int)statusCode
        };

        context.ExceptionHandled = true;
    }
}
