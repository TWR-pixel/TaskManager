using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Core.UseCases.Common.Repositories;

namespace TaskManager.Core.UseCases.Common.UnitOfWorks;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    public IRepositoryBase<UserTaskEntity> UserTasks { get; init; }
    public IRepositoryBase<TaskColumnEntity> UserTaskColumns { get; init; }
    public IRepositoryBase<RoleEntity> Roles { get; init; }
    public IRepositoryBase<UserEntity> Users { get; init; }
}
