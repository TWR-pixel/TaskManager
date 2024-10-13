using Ardalis.Specification;
namespace TaskManager.Core.Entities.Common;

public interface IRepositoryBaseCore<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class
{
}