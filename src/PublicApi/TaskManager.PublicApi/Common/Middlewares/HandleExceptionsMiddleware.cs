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
        catch (NotFoundException notFound)
        {
            _logger.LogError(notFound.Message, notFound);

            var problemDetails = new ProblemDetails
            {
                Title = "Not found",
                Detail = notFound.Message,
                Status = StatusCodes.Status404NotFound,
            };

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (UserAlreadyExistsException userAlreadyExists)
        {
            var alreadyExists = new ProblemDetails
            {
                Title = "User already exists",
                Detail = userAlreadyExists.Message,
                Status = StatusCodes.Status409Conflict
            };

            context.Response.StatusCode = StatusCodes.Status409Conflict;

            await context.Response.WriteAsJsonAsync(alreadyExists);
        }
        catch (NotRightException notRight)
        {
            var notRightValue = new ProblemDetails()
            {
                Title = "Not right value",
                Detail = notRight.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(notRightValue);
        }
        catch (NotVerifiedException notVerified)
        {
            var notVerifiedDetails = new ProblemDetails()
            {
                Title = "Not verified",
                Detail = notVerified.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(notVerifiedDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            
            var unknownError = new ProblemDetails
            {
                Title = "Unknown exception",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(unknownError);
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
