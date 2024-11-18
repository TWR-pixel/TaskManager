using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.UserTask;

public sealed record UserTaskDto
{
    [SetsRequiredMembers]
    public UserTaskDto(string? title,
                       string? description,
                       DateTime createdAt,
                       DateOnly? completedAt,
                       bool isCompleted,
                       bool isInProgress,
                       //int ownerId,
                       int userTaskColumnId,
                       int id)
    {
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        CompletedAt = completedAt;
        IsCompleted = isCompleted;
        IsInProgress = isInProgress;
        // OwnerId = ownerId;
        UserTaskColumnId = userTaskColumnId;
        Id = id;
    }

    public required int Id { get; set; }
    public required string? Title { get; set; }
    public required string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateOnly? CompletedAt { get; set; }

    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

    // public required int OwnerId { get; set; }
    public required int UserTaskColumnId { get; set; }
}
