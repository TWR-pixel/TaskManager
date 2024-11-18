using TaskManager.Domain.UseCases.Roles;
using TaskManager.Domain.UseCases.TaskColumns;
using TaskManager.Domain.UseCases.Tasks;
using TaskManager.Domain.UseCases.UserBoard;
using TaskManager.Domain.UseCases.Users;

namespace TaskManager.Domain.UseCases.Common.UnitOfWorks;

public interface IReadUnitOfWork
{
    public IUserTaskRepository UserTasks { get; init; }
    public IUserTaskColumnRepository UserTaskColumns { get; init; }
    public IRoleRepository Roles { get; init; }
    public IUserRepository Users { get; init; }
    public IUserBoardRepository UserBoards { get; init; }
}
