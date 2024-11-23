using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Infrastructure.Sqlite.Common;

namespace TaskManager.Infrastructure.Sqlite.UserTask;

public sealed class UserTaskRepository(TaskManagerDbContext dbContext) : RepositoryBase<UserTaskEntity>(dbContext), IUserTaskRepository
{
    public async Task<IEnumerable<UserTaskEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        var userTaskEntities = await DbContext.UserTasks
                .Include(u => u.TaskColumn)
            .ToListAsync(cancellationToken);

        return userTaskEntities;
    }

    public Task<UserTaskEntity?> GetByTitle(string title, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<UserTaskEntity?> GetWithUserTaskColumnById(int id, CancellationToken cancellationToken = default)
    {
        var userTaskEntity = await DbContext.UserTasks
                .Include(u => u.TaskColumn)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return userTaskEntity;
    }
}
