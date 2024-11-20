using MediatR;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Common.Requests;

/// <summary>
/// Base class for all request handlers
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class RequestHandlerBase<TRequest, TResponse>(IUnitOfWork unitOfWork) : IRequestHandler<TRequest, TResponse>
    where TRequest : RequestBase<TResponse>
    where TResponse : class
{
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    public async virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
