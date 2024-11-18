using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Common.Requests.Commands;

public abstract class CommandRequestHandlerBase<TRequest, TResponse>(IUnitOfWork unitOfWork) : RequestHandlerBase<TRequest, TResponse>(unitOfWork)
    where TRequest : CommandRequestBase<TResponse>
    where TResponse : class
{
}
