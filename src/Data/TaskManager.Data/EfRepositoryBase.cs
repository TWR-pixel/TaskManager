using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Data;

public abstract class EfRepositoryBase<T>(DbContext dbContext) : RepositoryBase<T>(dbContext) where T : class
{
}
