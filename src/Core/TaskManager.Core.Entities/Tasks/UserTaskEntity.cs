using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Domain.Entities.Tasks;

[Table("user_tasks")]
[Index(nameof(CreatedAt))]
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
    [Column("title")]
    public required string Title { get; set; }

    [MinLength(1)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("completedAt")]
    public DateOnly? CompletedAt { get; set; }

    [Column("is_completed")]
    public required bool IsCompleted { get; set; }

    [Column("is_in_progress")]
    public required bool IsInProgress { get; set; }

    [ForeignKey("owner_id")]
    public required UserEntity Owner { get; set; }

    [ForeignKey("task_column_id")]
    public required UserTaskColumnEntity TaskColumn { get; set; }
}
