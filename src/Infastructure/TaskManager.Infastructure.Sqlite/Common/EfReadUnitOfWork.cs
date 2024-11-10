using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Infrastructure.Sqlite.Common;

public sealed class EfReadUnitOfWork(IReadRepositoryBase<UserTaskEntity> userTasks,
                                     IReadRepositoryBase<UserTaskColumnEntity> userTaskColumns,
                                     IReadRepositoryBase<RoleEntity> roles,
                                     IReadRepositoryBase<UserEntity> users) : IReadUnitOfWork
{
    public IReadRepositoryBase<UserTaskEntity> UserTasks { get; init; } = userTasks;
    public IReadRepositoryBase<UserTaskColumnEntity> UserTaskColumns { get; init; } = userTaskColumns;
    public IReadRepositoryBase<RoleEntity> Roles { get; init; } = roles;
    public IReadRepositoryBase<UserEntity> Users { get; init; } = users;
}
