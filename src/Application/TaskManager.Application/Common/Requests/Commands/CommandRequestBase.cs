namespace TaskManager.Application.Common.Requests.Commands;

/// <summary>
/// Base class for queries that change the state of the data 
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public abstract record CommandRequestBase<TResponse> : RequestBase<TResponse>
    where TResponse : class
{
}
