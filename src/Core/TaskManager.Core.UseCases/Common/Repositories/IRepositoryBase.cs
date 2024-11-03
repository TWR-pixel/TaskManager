using TaskManager.Core.Entities.Common.Entities;

namespace TaskManager.Core.UseCases.Common.Repositories;

public interface IRepositoryBase<TEntity> : IReadRepositoryBase<TEntity>, Ardalis.Specification.IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}