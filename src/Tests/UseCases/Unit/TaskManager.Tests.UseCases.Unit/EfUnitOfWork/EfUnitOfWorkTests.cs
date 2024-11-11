using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Infrastructure.Sqlite;
using TaskManager.Infrastructure.Sqlite.Common;
using UnitOfWork = TaskManager.Infrastructure.Sqlite.Common.EfUnitOfWork;

namespace TaskManager.Tests.Infrastructure.Unit.EfUnitOfWork;

public class EfUnitOfWorkTests
{
    private readonly UnitOfWork _unitOfWork;
    private readonly EfRepository<RoleEntity> _testRepo;

    public EfUnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseInMemoryDatabase("taskManagerEfUOFDb")
            .Options;

        var dbContext = new TaskManagerDbContext(options);

        var userTasksRepo = new EfRepository<UserTaskEntity>(dbContext);
        var roleRepo = new EfRepository<RoleEntity>(dbContext);

        _testRepo = roleRepo;

        var userRepo = new EfRepository<UserEntity>(dbContext);
        var userTaskColumnsRepo = new EfRepository<UserTaskColumnEntity>(dbContext);

        _unitOfWork = new UnitOfWork(userTasksRepo, userTaskColumnsRepo, roleRepo, userRepo, dbContext);
    }

    [Fact]
    public async Task Test()
    {

    }
}
