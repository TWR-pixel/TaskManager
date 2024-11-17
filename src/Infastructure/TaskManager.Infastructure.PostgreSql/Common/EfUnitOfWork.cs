using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.Users;

namespace TaskManager.Infrastructure.PostgreSql.Common;

public sealed class EfUnitOfWork(TaskManagerDbContext dbContext,
                    IUserTaskRepository userTasks,
                    IUserTaskColumnRepository userTaskColumns,
                    IRoleRepository roles,
                    IUserRepository users) : IUnitOfWork
{
    public IUserTaskRepository UserTasks { get; init; } = userTasks;
    public IUserTaskColumnRepository UserTaskColumns { get; init; } = userTaskColumns;
    public IRoleRepository Roles { get; init; } = roles;
    public IUserRepository Users { get; init; } = users;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
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
