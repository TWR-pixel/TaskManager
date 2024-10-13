using System.Numerics;

namespace TaskManager.Core.Entities.Common;

/// <summary>
/// definices types for adding in db
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TID"></typeparam>
public interface IWriteOnlyRepositoryCore<TEntity, TID> where TEntity : EntityBase
    where TID : INumber<TID>
{
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity?> RemoveByIdAsync(TID id, CancellationToken cancellationToken = default);
    public Task<TEntity?> UpdateByIdAsync(TID id, CancellationToken cancellationToken = default);
    public Task<TEntity?> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
}
