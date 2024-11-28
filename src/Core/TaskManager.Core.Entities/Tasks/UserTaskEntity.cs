using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Domain.Entities.Tasks;

public sealed class UserTaskEntity : EntityBase
{
    [SetsRequiredMembers]
    public UserTaskEntity(bool isCompleted,
                          bool isInProgress,
                          UserEntity owner,
                          UserTaskColumnEntity taskColumn,
                          string title,
                          string description,
                          DateOnly? completedAt)
    {
        IsCompleted = isCompleted;
        IsInProgress = isInProgress;
        Owner = owner;
        TaskColumn = taskColumn;
        Title = title;
        Description = description;
        CompletedAt = completedAt;
    }

    public UserTaskEntity() { }

    [StringLength(256, MinimumLength = 1)]
    public required string Title { get; set; }

    [MinLength(1)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateOnly? CompletedAt { get; set; }

    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

    public required UserEntity Owner { get; set; }
    public required UserTaskColumnEntity TaskColumn { get; set; }
}
