using System.Diagnostics.CodeAnalysis;
using TaskManager.Core.Entities.Common.Entities;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.Tasks;

public sealed class UserTaskEntity : EntityBase
{
    [SetsRequiredMembers]
    public UserTaskEntity(bool isCompleted,
                          bool isInProgress,
                          UserEntity owner,
                          TaskColumnEntity taskColumn,
                          string title,
                          string content,
                          DateOnly? doTo)
    {
        IsCompleted = isCompleted;
        IsInProgress = isInProgress;
        Owner = owner;
        TaskColumn = taskColumn;
        Title = title;
        Content = content;
        DoTo = doTo;
    }

    public UserTaskEntity() { }

    public required string Title { get; set; }
    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateOnly? DoTo { get; set; }
    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

    public required UserEntity Owner { get; set; }
    public required TaskColumnEntity TaskColumn { get; set; }
}
