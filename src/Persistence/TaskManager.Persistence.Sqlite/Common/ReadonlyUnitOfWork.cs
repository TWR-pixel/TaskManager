using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.UserOrganization;
using TaskManager.Domain.UseCases.Users;

namespace TaskManager.Persistence.Sqlite.Common;

public sealed class ReadonlyUnitOfWork(IUserTaskRepository userTasks,
                      IUserTaskColumnRepository userTaskColumns,
                      IRoleRepository roles,
                      IUserRepository users,
                      IUserOrganizationRepository userOrganizations) : IReadonlyUnitOfWork
{
    public IUserTaskRepository UserTasks { get; init; } = userTasks;
    public IUserTaskColumnRepository UserTaskColumns { get; init; } = userTaskColumns;
    public IRoleRepository Roles { get; init; } = roles;
    public IUserRepository Users { get; init; } = users;
    public IUserOrganizationRepository UserOrganizations { get; init; } = userOrganizations;
}
