using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.UseCases.Common.Repositories;

public interface IRepositoryBase<TEntity> : IReadRepositoryBase<TEntity>
    where TEntity : EntityBase
{
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    public  Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    public  Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

}