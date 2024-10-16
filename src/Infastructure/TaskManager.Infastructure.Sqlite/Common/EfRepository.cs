using Ardalis.Specification.EntityFrameworkCore;
using TaskManager.Core.Entities.Common.Entities;
using TaskManager.Core.Entities.Common.Repositories;

namespace TaskManager.Infastructure.Sqlite.Common;

public sealed class EfRepository<TEntity>(TaskManagerDbContext dbContext) : RepositoryBase<TEntity>(dbContext), IRepositoryBaseCore<TEntity>
    where TEntity : EntityBase
{
}
