using TaskManager.Domain.UseCases.Common.UnitOfWorks;
using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.UserBoard;
using TaskManager.Domain.UseCases.Users;

namespace TaskManager.DALImplementation.Sqlite.Common;

public sealed class ReadUnitOfWork(IUserTaskRepository userTasks,
                      IUserTaskColumnRepository userTaskColumns,
                      IRoleRepository roles,
                      IUserRepository users,
                      IUserBoardRepository userBoards) : IReadUnitOfWork
{
    public IUserTaskRepository UserTasks { get; init; } = userTasks;
    public IUserTaskColumnRepository UserTaskColumns { get; init; } = userTaskColumns;
    public IRoleRepository Roles { get; init; } = roles;
    public IUserRepository Users { get; init; } = users;
    public IUserBoardRepository UserBoards { get; init; } = userBoards;
}
