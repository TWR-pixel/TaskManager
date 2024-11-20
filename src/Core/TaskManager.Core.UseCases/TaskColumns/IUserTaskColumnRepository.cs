using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.TaskColumns;

public interface IUserTaskColumnRepository : IRepository<UserTaskColumnEntity>
{
    public Task<UserTaskColumnEntity?> GetByIdWithOwnerAndUserTasksAsync(int id, CancellationToken cancellationToken = default);
    public Task<UserTaskColumnEntity?> GetByIdWithUserTasksAsync(int id, CancellationToken cancellationToken = default);
}
