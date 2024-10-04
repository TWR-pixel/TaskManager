using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Core.Entities.Tasks;

public sealed record UserTaskEntity : EntityBase
{
    public required string Name { get; set; }
    public required string Content { get; set; }

    public required TaskColumnEntity Column { get; set; }
}
