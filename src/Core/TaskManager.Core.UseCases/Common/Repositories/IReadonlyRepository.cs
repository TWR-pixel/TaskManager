using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.UseCases.Common.Repositories;

public interface IReadonlyRepository<TEntity>
    where TEntity : class, IEntity
{
    public Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<int> CountAsync(CancellationToken cancellationToken = default);
    public Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default);
}
