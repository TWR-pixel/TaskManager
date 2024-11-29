namespace TaskManager.Application.Common.Requests.Queries;

/// <summary>
/// Base class for queries that do not change the state of the data 
/// </summary>
/// <typeparam name="TResponse">The response that the <see cref="QueryHandlerBase{TRequest, TResponse}"/> will return</typeparam>
public abstract record QueryBase<TResponse> : RequestBase<TResponse>
    where TResponse : class
{
}
