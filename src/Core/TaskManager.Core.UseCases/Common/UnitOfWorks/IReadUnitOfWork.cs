using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.Common.UnitOfWorks;

public interface IReadUnitOfWork
{
    public IReadRepositoryBase<UserTaskEntity> UserTasks { get; init; }
    public IReadRepositoryBase<UserTaskColumnEntity> UserTaskColumns { get; init; }
    public IReadRepositoryBase<RoleEntity> Roles { get; init; }
    public IReadRepositoryBase<UserEntity> Users { get; init; }
}
