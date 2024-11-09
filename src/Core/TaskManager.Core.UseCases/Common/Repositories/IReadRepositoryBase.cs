using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.UseCases.Common.Repositories;

public interface IReadRepositoryBase<TEntity> : Ardalis.Specification.IReadRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}
