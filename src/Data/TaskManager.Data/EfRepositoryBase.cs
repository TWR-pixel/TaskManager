using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data;

public abstract class EfRepositoryBase<T>
    : RepositoryBase<T> where T : class
{
    protected EfRepositoryBase(DbContext dbContext) : base(dbContext)
    {
    }
}
