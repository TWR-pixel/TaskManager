using Ardalis.Specification;
using TaskManager.Core.Entities.Common.Entities;

namespace TaskManager.Core.Entities.Common.Repositories;

public interface IReadRepositoryBaseCore<TEntity> : IReadRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}
