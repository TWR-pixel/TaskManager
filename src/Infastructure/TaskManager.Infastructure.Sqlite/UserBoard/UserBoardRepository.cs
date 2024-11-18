using Microsoft.EntityFrameworkCore;
using TaskManager.DALImplementation.Sqlite.Common;
using TaskManager.Domain.Entities.UserBoard;
using TaskManager.Domain.UseCases.UserBoard;

namespace TaskManager.DALImplementation.Sqlite.UserBoard;

public sealed class UserBoardRepository(TaskManagerDbContext dbContext) : RepositoryBase<UserBoardEntity>(dbContext), IUserBoardRepository
{
    public async Task<IEnumerable<UserBoardEntity>> GetAllByOwnerId(int ownerId, CancellationToken cancellationToken = default)
    {
        var userBoardEntities = await DbContext.UserBoards
            //.Include(u => u.Owner)
            .Where(u => u.Id == ownerId)
            .ToArrayAsync(cancellationToken);

        return userBoardEntities;
    }
}
