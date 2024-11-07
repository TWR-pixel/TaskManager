﻿using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Tasks.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Tasks.Specifications;

namespace TaskManager.Application.UserTask.Requests.GetByTitle;

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
        var result = await UnitOfWork.UserTasks.SingleOrDefaultAsync(new ReadTaskByTitleSpecification(request.Title), cancellationToken)
            ?? throw new TaskNotFoundException(request.Title);

        var response = new GetTaskByTitleResponse(result.Title, result.Description);

        return response;
    }
}