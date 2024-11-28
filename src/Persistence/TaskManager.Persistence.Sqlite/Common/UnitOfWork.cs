using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.Users;

namespace TaskManager.Persistence.Sqlite.Common;

public sealed class UnitOfWork(TaskManagerDbContext dbContext,
                                 IUserTaskRepository userTasks,
                                 IUserTaskColumnRepository userTaskColumns,
                                 IRoleRepository roles,
                                 IUserRepository users) : IUnitOfWork
{
    private readonly TaskManagerDbContext _dbContext = dbContext;

    public IUserTaskRepository UserTasks { get; init; } = userTasks;
    public IUserTaskColumnRepository UserTaskColumns { get; init; } = userTaskColumns;
    public IRoleRepository Roles { get; init; } = roles;
    public IUserRepository Users { get; init; } = users;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>TransactionId</returns>
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
