using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Users.Exceptions;

namespace TaskManager.PublicApi.Common.Middlewares;

public sealed class HandleExceptionsMiddleware() : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException notFound)
        {
            await SendResponseAsync("Not found", notFound, StatusCodes.Status404NotFound, context);
        }
        catch (UserAlreadyExistsException userAlreadyExists)
        {
            await SendResponseAsync("User already exists", userAlreadyExists, StatusCodes.Status409Conflict, context);
        }
        catch (NotRightException notRight)
        {
            await SendResponseAsync("Not right value", notRight, StatusCodes.Status400BadRequest, context);
        }
        catch (NotVerifiedException notVerified)
        {
            await SendResponseAsync("Not verified", notVerified, StatusCodes.Status400BadRequest, context);
        }
        catch (Exception ex)
        {
            await SendResponseAsync("Unknown exception", ex, StatusCodes.Status400BadRequest, context);
        }
    }

    private static async Task SendResponseAsync(string title, Exception exception, int statusCode, HttpContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = exception.Message,
            Status = statusCode
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
