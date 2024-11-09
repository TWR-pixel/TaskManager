using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.Common.UnitOfWorks;

public interface IReadUnitOfWork
{
    public IReadRepositoryBase<UserTaskEntity> UserTasks { get; init; }
    public IReadRepositoryBase<TaskColumnEntity> UserTaskColumns { get; init; }
    public IReadRepositoryBase<RoleEntity> Roles { get; init; }
    public IReadRepositoryBase<UserEntity> Users { get; init; }
}
