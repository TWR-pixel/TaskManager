using Ardalis.Specification.EntityFrameworkCore;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Infrastructure.Sqlite.Common;

/// <summary>
/// Class doesnt save changes in database, use unit of work for this
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="dbContext"></param>
public sealed class EfRepository<TEntity>(TaskManagerDbContext dbContext) : RepositoryBase<TEntity>(dbContext), IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
    private readonly TaskManagerDbContext _dbContext = dbContext;

    public override async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        return result.Entity;
    }

    public override async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        return entities;
    }

    public override Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public override Task DeleteRangeAsync(Ardalis.Specification.ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        _dbContext.Set<TEntity>().RemoveRange(query);

        return Task.CompletedTask;
    }

    public override Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);

        return Task.CompletedTask;
    }

    //[Obsolete("This method is obsolete, use unit of work method SaveChanges")]
    //public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    throw new DoNotUseThisMethodException(nameof(SaveChangesAsync));
    //}

    public override Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Update(entity);

        return Task.CompletedTask;
    }

    public override Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);

        return Task.CompletedTask;
    }
}
