using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.UserOrganization;
using TaskManager.Domain.UseCases.Users;

namespace TaskManager.Domain.UseCases.Common.UnitOfWorks;

public interface IUnitOfWork
{
    public IUserTaskRepository UserTasks { get; init; }
    public IUserTaskColumnRepository UserTaskColumns { get; init; }
    public IRoleRepository Roles { get; init; }
    public IUserRepository Users { get; init; }
    public IUserOrganizationRepository UserOrganizations { get; init; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    public void CommitTransaction();
}
