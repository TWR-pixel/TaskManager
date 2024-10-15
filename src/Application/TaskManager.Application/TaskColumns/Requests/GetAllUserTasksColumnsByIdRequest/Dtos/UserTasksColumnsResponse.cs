using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record UserTasksColumnsResponse
{
    [SetsRequiredMembers]
    public UserTasksColumnsResponse(int id, string name, string content, IEnumerable<UserTaskResponse>? userTasksInColumn)
    {
        Id = id;
        Name = name;
        Content = content;
        UserTasksInColumn = userTasksInColumn;
    }

    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Content { get; set; }

    public IEnumerable<UserTaskResponse>? UserTasksInColumn { get; set; }
}
