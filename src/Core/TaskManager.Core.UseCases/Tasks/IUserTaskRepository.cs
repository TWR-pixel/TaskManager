using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.Tasks;

public interface IUserTaskRepository : IRepository<UserTaskEntity>
{
    public Task<UserTaskEntity?> GetWithUserTaskColumnById(int id, CancellationToken cancellationToken = default);
    public Task<UserTaskEntity?> GetByTitle(string title, CancellationToken cancellationToken = default);
    public Task<IEnumerable<UserTaskEntity>> GetAll(CancellationToken cancellationToken = default);
}