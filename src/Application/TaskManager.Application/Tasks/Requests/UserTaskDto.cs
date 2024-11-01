using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;

namespace TaskManager.Application.Tasks.Requests;

public sealed record UserTaskDto
{
    [SetsRequiredMembers]
    public UserTaskDto(string title,
                       string description,
                       DateTime createdAt,
                       DateOnly? complitedAt,
                       bool isCompleted,
                       bool isInProgress,
                      //int ownerId,
                       int userTaskColumnId)
    {
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        ComplitedAt = complitedAt;
        IsCompleted = isCompleted;
        IsInProgress = isInProgress;
       // OwnerId = ownerId;
        UserTaskColumnId = userTaskColumnId;
    }

    public required string Title { get; set; }
    public required string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateOnly? ComplitedAt { get; set; }

    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

   // public required int OwnerId { get; set; }
    public required int UserTaskColumnId { get; set; }
}
