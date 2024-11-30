using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Domain.Entities.TaskColumns;

public sealed class UserTaskColumnEntity : IEntity
{
    public int Id { get; set; }

    [StringLength(128, MinimumLength = 3)]
    public required string Title { get; set; }

    [StringLength(256)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IEnumerable<UserTaskEntity>? TasksInColumn { get; set; }

    public required UserEntity Owner { get; set; }

    [SetsRequiredMembers]
    public UserTaskColumnEntity(UserEntity owner, string title, string? description = null)
    {
        Owner = owner;
        Title = title;
        Description = description;
    }

    public UserTaskColumnEntity() { }
}
