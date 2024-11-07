using MediatR;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Common.Requests.Queries;

public abstract class QueryRequestHandlerBase<TRequest, TResponse>(IReadUnitOfWork unitOfWork) : IRequestHandler<TRequest, TResponse>
    where TRequest : QueryRequestBase<TResponse>
    where TResponse : class
{
    protected IReadUnitOfWork UnitOfWork { get; private set; } = unitOfWork;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
