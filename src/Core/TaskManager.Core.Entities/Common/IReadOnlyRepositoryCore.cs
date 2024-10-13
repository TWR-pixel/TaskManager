using System.Numerics;

namespace TaskManager.Core.Entities.Common;

public interface IReadOnlyRepositoryCore<TEntity, TID> where TEntity : EntityBase
    where TID : INumber<TID> // TID is number type
{
    public Task<TEntity?> FindByIdAsync(TID id, CancellationToken cancellationToken);

}
