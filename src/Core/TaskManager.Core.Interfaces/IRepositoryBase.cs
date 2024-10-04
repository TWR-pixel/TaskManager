using Ardalis.Specification;

namespace TaskManager.Core.Interfaces;

public interface IRepositoryBaseArdalis<T> : IRepositoryBase<T> where T : class
{
}
