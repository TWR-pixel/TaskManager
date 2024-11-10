using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Infrastructure.PostgreSql.Common;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext;

    public IRepositoryBase<UserTaskEntity> UserTasks { get; init; }
    public IRepositoryBase<UserTaskColumnEntity> UserTaskColumns { get; init; }
    public IRepositoryBase<RoleEntity> Roles { get; init; }
    public IRepositoryBase<UserEntity> Users { get; init; }

    public EfUnitOfWork(TaskManagerDbContext dbContext,
                        IRepositoryBase<UserTaskEntity> userTasks,
                        IRepositoryBase<UserTaskColumnEntity> userTaskColumns,
                        IRepositoryBase<RoleEntity> roles,
                        IRepositoryBase<UserEntity> users)
    {
        _dbContext = dbContext;
        UserTasks = userTasks;
        UserTaskColumns = userTaskColumns;
        Roles = roles;
        Users = users;
    }


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void CommitTransaction()
    {
        throw new NotImplementedException();
    }
}
