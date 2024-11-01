using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record UserTaskColumnsResponse
{
    [SetsRequiredMembers]
    public UserTaskColumnsResponse(int id, string title, string content, IEnumerable<UserTaskResponse>? tasks, int sequence)
    {
        Id = id;
        Title = title;
        Content = content;
        Tasks = tasks;
    }

    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }

    public IEnumerable<UserTaskResponse>? Tasks { get; set; }
}
