using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.Tasks.Specifications;

namespace TaskManager.Application.Tasks.Requests.GetTaskByTitleRequest;

public sealed record GetTaskByTitleRequest : RequestBase<GetTaskByTitleResponse>
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

public sealed class GetTaskByTitleRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetTaskByTitleRequest, GetTaskByTitleResponse>(unitOfWork)
{
    public override async Task<GetTaskByTitleResponse> Handle(GetTaskByTitleRequest request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.UserTasks.SingleOrDefaultAsync(new GetTaskByTitleSpecification(request.Title), cancellationToken)
            ?? throw new EntityNotFoundException($"User task with title '{request.Title} not found");

        var response = new GetTaskByTitleResponse(result.Title, result.Content);

        return response;
    }
}
