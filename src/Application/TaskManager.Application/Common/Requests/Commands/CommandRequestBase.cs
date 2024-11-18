namespace TaskManager.Application.Common.Requests.Commands;

public abstract record CommandRequestBase<TResponse> : RequestBase<TResponse>
    where TResponse : class
{
}
