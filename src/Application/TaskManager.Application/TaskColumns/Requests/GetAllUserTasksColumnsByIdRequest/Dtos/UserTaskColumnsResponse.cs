using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record UserTaskColumnsResponse
{
    [SetsRequiredMembers]
    public UserTaskColumnsResponse(int id, string name, string content, IEnumerable<UserTaskResponse>? tasks)
    {
        Id = id;
        Name = name;
        Content = content;
        Tasks = tasks;
    }

    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Content { get; set; }

    public IEnumerable<UserTaskResponse>? Tasks { get; set; }
}
