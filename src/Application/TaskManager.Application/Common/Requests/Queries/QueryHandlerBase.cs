using MediatR;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Common.Requests.Queries;

public abstract class QueryHandlerBase<TRequest, TResponse>(IReadonlyUnitOfWork unitOfWork) : IRequestHandler<TRequest, TResponse>
    where TRequest : QueryBase<TResponse>
    where TResponse : class
{
    protected IReadonlyUnitOfWork UnitOfWork { get; private set; } = unitOfWork;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
