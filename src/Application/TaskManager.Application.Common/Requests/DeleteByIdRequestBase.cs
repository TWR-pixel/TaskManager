namespace TaskManager.Application.Common.Requests;

public abstract record DeleteByIdRequestBase<TResponse> : RequestBase<TResponse>
    where TResponse : class
{
    public required int Id { get; set; }
}
