using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.TaskColumns;

public sealed class TaskColumnEntity : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<TaskEntity>? TasksInColumn { get; set; }
    public required UserEntity Owner { get; set; }
}
