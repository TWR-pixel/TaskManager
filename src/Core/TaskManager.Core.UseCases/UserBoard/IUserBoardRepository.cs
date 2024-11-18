using TaskManager.Domain.Entities.UserBoard;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.UserBoard;

public interface IUserBoardRepository : IRepositoryBase<UserBoardEntity>
{
    public Task<IEnumerable<UserBoardEntity>> GetAllByOwnerId(int ownerId, CancellationToken cancellationToken = default);
}
