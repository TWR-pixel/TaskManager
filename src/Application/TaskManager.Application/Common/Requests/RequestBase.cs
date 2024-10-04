using MediatR;

namespace TaskManager.Application.Common.Requests;

public abstract class RequestBase<TResponse> : IRequest<TResponse> where TResponse : class
{
}
