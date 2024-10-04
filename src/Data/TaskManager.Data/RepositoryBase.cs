using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data;

public abstract class RepositoryBase<T>
    : Ardalis.Specification.EntityFrameworkCore.RepositoryBase<T> where T : class
{
    protected RepositoryBase(DbContext dbContext) : base(dbContext)
    {
    }
}
