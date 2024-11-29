namespace TaskManager.Application.Common.Requests.Commands;

/// <summary>
/// Base class for queries that change the state of the data 
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public abstract record CommandBase<TResponse> : RequestBase<TResponse>
    where TResponse : class
{
}
