using Ardalis.Specification.EntityFrameworkCore;
using TaskManager.Core.Entities.Common;

namespace TaskManager.Infastructure.Common;

public sealed class EfRepository<TEntity>(TaskManagerDbContext dbContext) : RepositoryBase<TEntity>(dbContext), IRepositoryBaseCore<TEntity>
    where TEntity : EntityBase
{
}
