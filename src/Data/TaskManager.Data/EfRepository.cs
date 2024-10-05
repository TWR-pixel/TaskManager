namespace TaskManager.Data;

public sealed class EfRepository<T>(TaskManagerDbContext dbContext) : EfRepositoryBase<T>(dbContext) where T : class
{
}
