using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Infrastructure.Sqlite.Common;

public sealed class EfUnitOfWork(IRepositoryBase<UserTaskEntity> userTasks,
                    IRepositoryBase<TaskColumnEntity> userTaskColumns,
                    IRepositoryBase<RoleEntity> roles,
                    IRepositoryBase<UserEntity> users) : IUnitOfWork
{
    public IRepositoryBase<UserTaskEntity> UserTasks { get; init; } = userTasks;
    public IRepositoryBase<TaskColumnEntity> UserTaskColumns { get; init; } = userTaskColumns;
    public IRepositoryBase<RoleEntity> Roles { get; init; } = roles;
    public IRepositoryBase<UserEntity> Users { get; init; } = users;
}
