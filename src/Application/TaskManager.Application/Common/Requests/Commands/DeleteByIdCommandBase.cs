namespace TaskManager.Application.Common.Requests.Commands;

public sealed record DeleteByIdCommandBase<TResponse> : CommandBase<TResponse>
    where TResponse : class
{
    public required int Id { get; set; }    
}
