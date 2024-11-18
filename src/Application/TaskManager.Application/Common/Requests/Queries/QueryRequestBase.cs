namespace TaskManager.Application.Common.Requests.Queries;

public abstract record QueryRequestBase<TResponse> : RequestBase<TResponse>
    where TResponse : class
{
}
