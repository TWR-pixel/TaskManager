using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace TaskManager.Application.Common.Requests.Handlers;

public class RequestExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : RequestBase<TResponse>
    where TException : Exception
    where TResponse : class
{
    private readonly ILogger<RequestExceptionHandler<TRequest, TResponse, TException>> _logger;

    public RequestExceptionHandler(ILogger<RequestExceptionHandler<TRequest, TResponse, TException>> logger)
    {
        _logger = logger;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        _logger.LogInformation(exception, "Something went wrong with message '{message}'", exception.Message);

        return Task.CompletedTask;
    }
}
