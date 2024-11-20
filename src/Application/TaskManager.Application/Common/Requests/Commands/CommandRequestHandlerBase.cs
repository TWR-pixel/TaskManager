using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Common.Requests.Commands;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="unitOfWork"></param>
public abstract class CommandRequestHandlerBase<TRequest, TResponse>(IUnitOfWork unitOfWork) : RequestHandlerBase<TRequest, TResponse>(unitOfWork)
    where TRequest : CommandRequestBase<TResponse>
    where TResponse : class
{
}
