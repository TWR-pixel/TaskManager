using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.Tasks.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserTask.Requests.Queries;

public sealed record GetTaskByTitleQuery : QueryBase<GetTaskByTitleResponse>
{
    public required string Title { get; set; }
}

public sealed record GetTaskByTitleResponse : ResponseBase
{
    [SetsRequiredMembers]
    public GetTaskByTitleResponse(string title, string? description)
    {
        Title = title;
        Description = description;
    }

    public required string Title { get; set; }
    public string? Description { get; set; }
}

public sealed class GetTaskByTitleQueryHandler(IReadonlyUnitOfWork unitOfWork) : QueryHandlerBase<GetTaskByTitleQuery, GetTaskByTitleResponse>(unitOfWork)
{
    public override async Task<GetTaskByTitleResponse> Handle(GetTaskByTitleQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.UserTasks.GetByTitle(request.Title, cancellationToken)
            ?? throw new TaskNotFoundException(request.Title);

        var response = new GetTaskByTitleResponse(result.Title, result.Description);

        return response;
    }
}
