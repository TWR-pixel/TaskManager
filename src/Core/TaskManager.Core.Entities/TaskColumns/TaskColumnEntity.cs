using System.Diagnostics.CodeAnalysis;
using TaskManager.Core.Entities.Common.Entities;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.TaskColumns;

public sealed class TaskColumnEntity : EntityBase
{
    [SetsRequiredMembers]
    public TaskColumnEntity(UserEntity owner, string name)
    {
        Owner = owner;
        Name = name;
    }

    public TaskColumnEntity()
    {
        
    }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<UserTaskEntity>? TasksInColumn { get; set; }
    public required UserEntity Owner { get; set; }
}
