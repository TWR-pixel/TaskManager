using MediatR;

namespace TaskManager.Application.Common.Requests;

public abstract record RequestBase<TResponse> : IRequest<TResponse> where TResponse : class
{
}
