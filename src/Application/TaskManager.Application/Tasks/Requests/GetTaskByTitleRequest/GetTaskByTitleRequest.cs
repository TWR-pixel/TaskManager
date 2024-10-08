﻿using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Data;
using TaskManager.Data.Task.Specifications;

namespace TaskManager.Application.Tasks.Requests.GetTaskByTitleRequest;

public sealed class GetTaskByTitleRequest : RequestBase<GetTaskByTitleResponse>
{
    public required string Title { get; set; }
}

public sealed class GetTaskByTitleResponse : ResponseBase
{
}

public sealed class GetTaskByTitleRequestHandler : RequestHandlerBase<GetTaskByTitleRequest, GetTaskByTitleResponse>
{
    private readonly EfRepositoryBase<UserTaskEntity> _tasksRepo;

    public GetTaskByTitleRequestHandler(EfRepositoryBase<UserTaskEntity> tasksRepo)
    {
        _tasksRepo = tasksRepo;
    }

    public override async Task<GetTaskByTitleResponse> Handle(GetTaskByTitleRequest request, CancellationToken cancellationToken)
    {
        var result = await _tasksRepo.SingleOrDefaultAsync(new FindTaskByTitleSpecificationQuery(request.Title), cancellationToken);

        var response = new GetTaskByTitleResponse();

        return response;
    }
}
