using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.TaskColumn.Requests.GetAllUserTasksColumnsByIdRequest.Dtos;

public sealed record UserTaskResponse
{
    [SetsRequiredMembers]
    public UserTaskResponse(string name,
                            string? description,
                            bool isInProgress,
                            bool isCompleted,
                            DateTime createdAt,
                            DateOnly? completedAt,
                            int id,
                            int columnId)
    {
        Name = name;
        Description = description;
        IsInProgress = isInProgress;
        IsCompleted = isCompleted;
        CreatedAt = createdAt;
        CompletedAt = completedAt;
        Id = id;
        ColumnId = columnId;
    }

    public required int Id { get; set; }
    public required int ColumnId { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required bool IsInProgress { get; set; }
    public required bool IsCompleted { get; set; }

    public required DateTime CreatedAt { get; set; }
    public DateOnly? CompletedAt { get; set; }
}