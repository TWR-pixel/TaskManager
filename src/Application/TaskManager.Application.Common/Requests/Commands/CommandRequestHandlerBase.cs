using MediatR;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Common.Requests.Commands;

public abstract class CommandRequestHandlerBase<TRequest, TResponse>(IUnitOfWork unitOfWork) : IRequestHandler<TRequest, TResponse>
    where TRequest : CommandRequestBase<TResponse>
    where TResponse : class
{
    protected IUnitOfWork UnitOfWork { get; private set; } = unitOfWork;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
