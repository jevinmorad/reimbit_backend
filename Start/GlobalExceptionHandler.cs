using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Start;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails();
        problemDetails.Status = StatusCodes.Status500InternalServerError;
        problemDetails.Title = "Internal Server Error";

        if (exception is SqlException)
        {
            problemDetails.Title = "Database operation failed";
        }
        else if (exception is ValidationException)
        {
            var validationException = exception as ValidationException;

            var errors = validationException.Errors.Select(c => new
            {
                code = c.PropertyName,
                description = new[] { c.ErrorMessage }
            }).ToDictionary(c => c.code, c => c.description);

            problemDetails.Status = StatusCodes.Status400BadRequest;
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                modelErrors = true,
                title = "One or more validation errors occurred",
                errors,
                status = StatusCodes.Status400BadRequest
            }, cancellationToken);

            return true;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
