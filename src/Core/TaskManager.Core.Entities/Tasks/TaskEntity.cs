using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Core.Entities.Tasks;

public class TaskEntity : EntityBase
{
    public required string Title { get; set; }
    public required string Content { get; set; }

    public required bool IsCompleted { get; set; }
    public required bool IsInProgress { get; set; }

    public required TaskColumnEntity Column { get; set; }
}
