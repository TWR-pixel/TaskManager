using Ardalis.Specification.EntityFrameworkCore;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Infrastructure.PostgreSql.Common;

public sealed class EfRepository<TEntity>(TaskManagerDbContext dbContext) : RepositoryBase<TEntity>(dbContext), IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}
