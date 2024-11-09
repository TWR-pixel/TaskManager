using Ardalis.Specification.EntityFrameworkCore;

namespace TaskManager.Infrastructure.PostgreSql.Common;

public sealed class EfRepository<TEntity>(TaskManagerDbContext dbContext) : RepositoryBase<TEntity>(dbContext), IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}
