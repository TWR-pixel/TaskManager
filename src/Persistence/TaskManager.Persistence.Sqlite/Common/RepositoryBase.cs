using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Persistence.Sqlite.Common;

/// <summary>
/// Class doesnt save changes in database, use unit of work for this
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="dbContext"></param>
public abstract class RepositoryBase<TEntity>(TaskManagerDbContext dbContext) : IRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly TaskManagerDbContext DbContext = dbContext;

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        return result.Entity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        return entities;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().RemoveRange(entities);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Update(entity);

        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().UpdateRange(entities);

        return Task.CompletedTask;
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await DbContext.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        var any = await DbContext.Set<TEntity>().AnyAsync(cancellationToken);

        return any;
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var count = await DbContext.Set<TEntity>().CountAsync(cancellationToken);

        return count;
    }

    public virtual async Task<IEnumerable<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await DbContext.Set<TEntity>().ToListAsync(cancellationToken);

        return entities;
    }
}
