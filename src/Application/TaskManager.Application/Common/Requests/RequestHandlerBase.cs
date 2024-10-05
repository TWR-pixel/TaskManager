using MediatR;

namespace TaskManager.Application.Common.Requests;

/// <summary>
/// Базовый обработчик для всех запросов
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : RequestBase<TResponse>
    where TResponse : ResponseBase
{
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
