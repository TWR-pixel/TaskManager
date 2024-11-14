using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Entities.Common.Exceptions;
using TaskManager.Domain.Entities.Users.Exceptions;

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
        catch (ValidationException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors has occurred",
            };

            if (ex.Data is not null)
            {
                problemDetails.Extensions["errors"] = ex.Data;
            }

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            await SendResponseAsync("Exception", ex, StatusCodes.Status400BadRequest, context);
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
