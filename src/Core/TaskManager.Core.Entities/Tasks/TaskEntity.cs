using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.Tasks;

public sealed class TaskEntity : EntityBase
{
    public required string Title { get; set; }
    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

    public required UserEntity Owner { get; set; }
    public required TaskColumnEntity TaskColumn { get; set; }
}
