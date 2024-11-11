using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.TaskColumn.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record UserTaskColumnsResponse
{
    [SetsRequiredMembers]
    public UserTaskColumnsResponse(int id, string title, string content, IEnumerable<UserTaskResponse>? tasks, int ordering)
    {
        Id = id;
        Title = title;
        Content = content;
        Tasks = tasks;
        Ordering = ordering;
    }

    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required int Ordering { get; set; }

    public IEnumerable<UserTaskResponse>? Tasks { get; set; }
}
