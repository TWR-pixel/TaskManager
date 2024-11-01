using TaskManager.Application.Common.Requests;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record GetAllUserTaskColumnsByIdWithTasksResponse : ResponseBase
{
    public required string Username { get; set; }

    public required IEnumerable<UserTaskColumnDto> TaskColumns { get; set; }
}
