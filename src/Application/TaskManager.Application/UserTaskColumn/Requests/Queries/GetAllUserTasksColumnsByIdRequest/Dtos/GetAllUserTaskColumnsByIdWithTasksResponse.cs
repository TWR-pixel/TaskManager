﻿using TaskManager.Application.Common.Requests;

namespace TaskManager.Application.UserTaskColumn.Requests.Queries.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record GetAllUserTaskColumnsByIdWithTasksResponse : ResponseBase
{
    public required string Username { get; set; }

    public required IEnumerable<UserTaskColumnsResponse> TaskColumns { get; set; }
}
