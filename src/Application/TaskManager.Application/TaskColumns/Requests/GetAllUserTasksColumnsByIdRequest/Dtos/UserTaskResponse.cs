using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record UserTaskResponse
{
    [SetsRequiredMembers]
    public UserTaskResponse(string name, string content, bool isInProgress, bool isCompleted, DateTime createdAt, DateOnly? doTo)
    {
        Name = name;
        Content = content;
        IsInProgress = isInProgress;
        IsCompleted = isCompleted;
        CreatedAt = createdAt;
        DoTo = doTo;
    }

    public required string Name { get; set; }
    public required string Content { get; set; }
    public required bool IsInProgress { get; set; }
    public required bool IsCompleted { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateOnly? DoTo { get; set; }
}