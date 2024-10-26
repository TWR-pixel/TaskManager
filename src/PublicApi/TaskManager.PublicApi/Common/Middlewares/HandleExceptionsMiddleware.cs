using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;

namespace TaskManager.PublicApi.Common.Middlewares;

public sealed class HandleExceptionsMiddleware(ILogger<HandleExceptionsMiddleware> logger) : IMiddleware
{
    private readonly ILogger<HandleExceptionsMiddleware> _logger = logger;

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

            context.Response.StatusCode = StatusCodes.Status404NotFound;

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

            context.Response.StatusCode = StatusCodes.Status409Conflict;

            await context.Response.WriteAsJsonAsync(alreadyExistsProblemDetails);
        }
        catch (NotRightCodeException ex)
        {
            var notRightCode = new ProblemDetails()
            {
                Title = "Not right code from email",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(notRightCode);
        }
        catch (CodeNotVerifiedException codeNotVerifiedEx)
        {
            var notRightCode = new ProblemDetails()
            {
                Title = "Not right code from email",
                Detail = codeNotVerifiedEx.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(notRightCode);
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

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(unhandledException);
        }
    }

    private async Task SendResponseAsync(string title, string detail, int Status, HttpContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = Status
        };

        context.Response.StatusCode = Status;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
