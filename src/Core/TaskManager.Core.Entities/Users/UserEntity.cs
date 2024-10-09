using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Entities.Users;

public sealed class UserEntity : EntityBase
{
    public required string EmailLogin { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public required string RefreshToken { get; set; }

    public IEnumerable<TaskColumnEntity>? TaskColumns { get; set; }
    public IEnumerable<UserTaskEntity>? Tasks { get; set; }

    public required RoleEntity Role { get; set; }
}
