using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.UseCases.Common.Repositories;

public interface IRepository<TEntity> : IReadonlyRepository<TEntity>, IWritableRepository<TEntity>
    where TEntity : EntityBase
{


}