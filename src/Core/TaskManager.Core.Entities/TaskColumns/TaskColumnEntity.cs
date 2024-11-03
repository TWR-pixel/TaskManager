using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Core.Entities.Common.Entities;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.TaskColumns;

[Table("task_columns")]
public sealed class TaskColumnEntity : EntityBase
{
    [SetsRequiredMembers]
    public TaskColumnEntity(UserEntity owner, string name, string? description = null)
    {
        Owner = owner;
        Name = name;
        Description = description;
    }

    public TaskColumnEntity() { }

    [StringLength(128, MinimumLength = 3)]
    [Column("name")]
    public required string Name { get; set; }

    [StringLength(256)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<UserTaskEntity>? TasksInColumn { get; set; }

    [ForeignKey("owner_id")]
    public required UserEntity Owner { get; set; }
}
