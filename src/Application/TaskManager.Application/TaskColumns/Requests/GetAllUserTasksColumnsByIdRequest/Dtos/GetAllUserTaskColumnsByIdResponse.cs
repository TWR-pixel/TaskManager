using TaskManager.Application.Common.Requests;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record GetAllUserTaskColumnsByIdResponse : ResponseBase
{
    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required IEnumerable<UserTasksColumnsResponse> UserTaskColumns { get; set; }
}
