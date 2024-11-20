using MediatR;

namespace TaskManager.Application.Common.Requests;

/// <summary>
/// Wrapper for MediatR <see cref="IRequest{TResponse}"/>
/// </summary>
/// <typeparam name="TResponse">The Response that the <see cref="RequestHandlerBase{TRequest, TResponse}"/> will return </typeparam>
public abstract record RequestBase<TResponse> : IRequest<TResponse> where TResponse : class
{
}
