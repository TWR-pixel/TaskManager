using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Infrastructure.Sqlite.Common;

public sealed class EfUnitOfWork(IRepositoryBase<UserTaskEntity> userTasks,
                    IRepositoryBase<UserTaskColumnEntity> userTaskColumns,
                    IRepositoryBase<RoleEntity> roles,
                    IRepositoryBase<UserEntity> users,
                    TaskManagerDbContext dbContext) : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext = dbContext;

    public IRepositoryBase<UserTaskEntity> UserTasks { get; init; } = userTasks;
    public IRepositoryBase<UserTaskColumnEntity> UserTaskColumns { get; init; } = userTaskColumns;
    public IRepositoryBase<RoleEntity> Roles { get; init; } = roles;
    public IRepositoryBase<UserEntity> Users { get; init; } = users;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>TransactionId </returns>
    public async Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        return result.TransactionId;
    }   

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void CommitTransaction()
    {
        _dbContext.Database.CommitTransaction();
    }
}
