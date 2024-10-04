using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Core.Entities.Users;

public sealed record UserEntity : EntityBase
{
    public required string LoginEmail { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }

    public IList<TaskColumnEntity> TaskColumns { get; set; } = [];
}
