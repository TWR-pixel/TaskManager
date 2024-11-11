using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Infrastructure.Sqlite;
using TaskManager.Infrastructure.Sqlite.Common;

namespace TaskManager.Tests.Unit.Common;

public class TestInitializer
{
    public DbContextOptions<TaskManagerDbContext> DefaultOptions { get; set; } = InitOptions();

    public static DbContextOptions<TaskManagerDbContext> InitOptions()
    {
        var randomDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseInMemoryDatabase(randomDbName)
            .Options;

        return options;
    }

    public static TaskManagerDbContext InitDbContext(DbContextOptions<TaskManagerDbContext> options)
    {
        var dbContext = new TaskManagerDbContext(options);

        return dbContext;
    }

    public static EfRepository<RoleEntity> InitRepo(TaskManagerDbContext dbContext)
    {
        var repo = new EfRepository<RoleEntity>(dbContext);

        return repo;
    }

    public static EfUnitOfWork InitUOF(TaskManagerDbContext dbContext)
    {
        var _testRepository = new EfRepository<RoleEntity>(dbContext);

        var userTasksRepo = new EfRepository<UserTaskEntity>(dbContext);
        var usersRepo = new EfRepository<UserEntity>(dbContext);
        var userTaskColumnRepo = new EfRepository<UserTaskColumnEntity>(dbContext);

        var uof = new EfUnitOfWork(userTasksRepo, userTaskColumnRepo, _testRepository, usersRepo, dbContext);

        return uof;
    }

}
