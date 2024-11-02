using Ardalis.Specification.EntityFrameworkCore;
using TaskManager.Core.Entities.Common.Entities;
using TaskManager.Core.UseCases.Common.Repositories;

namespace TaskManager.Infrastructure.Sqlite.Common;

public sealed class EfRepository<TEntity>(TaskManagerDbContext dbContext) : RepositoryBase<TEntity>(dbContext), IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}
