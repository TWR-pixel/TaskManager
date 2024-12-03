namespace TaskManager.Application.Common.Requests.Queries;

public sealed record GetByIdQueryBase<TResponse> : QueryBase<TResponse>
    where TResponse : class
{
    public required int Id { get; set; }
}
