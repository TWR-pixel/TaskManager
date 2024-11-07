using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.UseCases.Common.Repositories;

namespace TaskManager.Core.UseCases.Common.UnitOfWorks;

public interface IReadUnitOfWork
{
    public IReadRepositoryBase<UserTaskEntity> UserTasks { get; init; }
    public IReadRepositoryBase<TaskColumnEntity> UserTaskColumns { get; init; }
    public IReadRepositoryBase<RoleEntity> Roles { get; init; }
    public IReadRepositoryBase<UserEntity> Users { get; init; }
}
