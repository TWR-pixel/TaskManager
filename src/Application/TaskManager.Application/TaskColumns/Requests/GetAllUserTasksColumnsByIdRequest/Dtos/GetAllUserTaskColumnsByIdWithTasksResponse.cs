namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record GetAllUserTaskColumnsByIdWithTasksResponse : ResponseBase
{
    public required string Username { get; set; }

    public required IEnumerable<UserTaskColumnsResponse> TaskColumns { get; set; }
}
