using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.Users;

public interface IUserRepository : IRepository<UserEntity>
{
    public Task<UserEntity?> GetAllUserTaskColumnsWithTasksByIdAsync(int userId, CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetAllUserTasksByIdAsync(int userId, CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetWithRoleByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<UserEntity?> GetAllUserTasks(int id, CancellationToken cancellationToken = default);
}
