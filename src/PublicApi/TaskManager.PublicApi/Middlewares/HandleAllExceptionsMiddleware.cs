using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common;
using TaskManager.Application.Users.Requests.AuthenticateUserRequest;

namespace TaskManager.PublicApi.Middlewares;

public sealed class HandleAllExceptionsMiddleware(ILogger<HandleAllExceptionsMiddleware> logger) : IMiddleware
{
    private readonly ILogger<HandleAllExceptionsMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (EntityNotFoundException entityNotFoundException)
        {
            _logger.LogError(entityNotFoundException.Message, entityNotFoundException);

            var problemDetails = new ProblemDetails
            {
                Title = "Not found",
                Detail = entityNotFoundException.Message,
                Status = StatusCodes.Status404NotFound,
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (UserAlreadyExistsException userAlreadyExistsException)
        {
            var alreadyExistsProblemDetails = new ProblemDetails
            {
                Title = "User already exists",
                Detail = userAlreadyExistsException.Message,
                Status = StatusCodes.Status409Conflict
            };

            await context.Response.WriteAsJsonAsync(alreadyExistsProblemDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);

            var unhandledException = new ProblemDetails
            {
                Title = "Unhandled exception",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            };

            await context.Response.WriteAsJsonAsync(unhandledException);
        }
    }

    private ProblemDetails CreateProblemDetails(string title, string details, int statusCode)
    {
        var result = new ProblemDetails
        {
            Title = title,
            Detail = details,
            Status = statusCode
        };

        return result;
    }
}
