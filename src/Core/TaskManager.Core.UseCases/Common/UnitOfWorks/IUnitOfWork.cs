using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.Common.UnitOfWorks;

public interface IUnitOfWork
{
    public IRepositoryBase<UserTaskEntity> UserTasks { get; init; }
    public IRepositoryBase<UserTaskColumnEntity> UserTaskColumns { get; init; }
    public IRepositoryBase<RoleEntity> Roles { get; init; }
    public IRepositoryBase<UserEntity> Users { get; init; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    public void CommitTransaction();
}
