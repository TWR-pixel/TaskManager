using Ardalis.Specification;
using TaskManager.Core.Entities.Common.Entities;

namespace TaskManager.Core.Entities.Common.Repositories;

public interface IRepositoryBaseCore<TEntity> : IReadRepositoryBaseCore<TEntity>, IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}