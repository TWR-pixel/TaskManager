using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Infrastructure.Sqlite.Common;

public sealed class EfReadUnitOfWork(IReadRepositoryBase<UserTaskEntity> userTasks,
                                     IReadRepositoryBase<TaskColumnEntity> userTaskColumns,
                                     IReadRepositoryBase<RoleEntity> roles,
                                     IReadRepositoryBase<UserEntity> users) : IReadUnitOfWork
{
    public IReadRepositoryBase<UserTaskEntity> UserTasks { get; init; } = userTasks;
    public IReadRepositoryBase<TaskColumnEntity> UserTaskColumns { get; init; } = userTaskColumns;
    public IReadRepositoryBase<RoleEntity> Roles { get; init; } = roles;
    public IReadRepositoryBase<UserEntity> Users { get; init; } = users;
}
