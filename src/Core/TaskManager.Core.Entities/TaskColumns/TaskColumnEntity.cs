using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Entities.TaskColumns;

public sealed record TaskColumnEntity : EntityBase
{
    public required string Name { get; set; }
    public IList<UserTaskEntity> UserTasks { get; set; } = [];

}
