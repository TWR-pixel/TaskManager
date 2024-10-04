namespace TaskManager.Data;

public class EfRepository<T> : EfRepositoryBase<T> where T : class
{
    public EfRepository(TaskManagerDbContext dbContext) : base(dbContext)
    {
    }
}
