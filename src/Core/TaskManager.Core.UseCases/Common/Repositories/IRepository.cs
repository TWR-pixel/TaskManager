using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.UseCases.Common.Repositories;

public interface IRepository<TEntity> : IReadonlyRepository<TEntity>, IWriteonlyRepository<TEntity>
    where TEntity :class, IEntity
{


}