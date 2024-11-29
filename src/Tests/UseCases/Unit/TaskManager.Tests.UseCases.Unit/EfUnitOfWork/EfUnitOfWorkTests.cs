using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using UnitOfWork = TaskManager.Persistence.Sqlite.Common.UnitOfWork;

namespace TaskManager.Tests.Infrastructure.Unit.EfUnitOfWork;

public class EfUnitOfWorkTests
{
    private readonly UnitOfWork _unitOfWork;
    private readonly RepositoryBase<RoleEntity> _testRepo;

    public EfUnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseInMemoryDatabase("taskManagerEfUOFDb")
            .Options;

        var dbContext = new TaskManagerDbContext(options);

        var userTasksRepo = new RepositoryBase<UserTaskEntity>(dbContext);
        var roleRepo = new RepositoryBase<RoleEntity>(dbContext);

        _testRepo = roleRepo;

        var userRepo = new RepositoryBase<UserEntity>(dbContext);
        var userTaskColumnsRepo = new RepositoryBase<UserTaskColumnEntity>(dbContext);

        _unitOfWork = new UnitOfWork(userTasksRepo, userTaskColumnsRepo, roleRepo, userRepo, dbContext);
    }

    [Fact]
    public async Task Test()
    {

    }
}
