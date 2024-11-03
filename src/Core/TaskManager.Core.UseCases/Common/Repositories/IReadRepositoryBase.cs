using TaskManager.Core.Entities.Common.Entities;

namespace TaskManager.Core.UseCases.Common.Repositories;

public interface IReadRepositoryBase<TEntity> : Ardalis.Specification.IReadRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}
