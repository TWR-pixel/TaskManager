using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;
using TaskManager.Infrastructure.Sqlite;
using TaskManager.Infrastructure.Sqlite.Common;

namespace TaskManager.Tests.Infrastructure.Unit;

public class TestInitializer
{
    #region Private

    public static DbContextOptions<TaskManagerDbContext> InitTestOptions()
    {
        var randomDbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseInMemoryDatabase(randomDbName)
            .Options;

        return options;
    }

    public static TaskManagerDbContext InitTestDbContext(DbContextOptions<TaskManagerDbContext> options)
    {
        var dbContext = new TaskManagerDbContext(options);

        return dbContext;
    }

    public static EfRepository<RoleEntity> InitTestRepo(TaskManagerDbContext dbContext)
    {
        var repo = new EfRepository<RoleEntity>(dbContext);

        return repo;
    }

    public static EfUnitOfWork InitTestUnitOfWork(TaskManagerDbContext dbContext)
    {
        var _testRepository = new EfRepository<RoleEntity>(dbContext);

        var userTasksRepo = new EfRepository<UserTaskEntity>(dbContext);
        var usersRepo = new EfRepository<UserEntity>(dbContext);
        var userTaskColumnRepo = new EfRepository<UserTaskColumnEntity>(dbContext);

        var uof = new EfUnitOfWork(userTasksRepo, userTaskColumnRepo, _testRepository, usersRepo, dbContext);

        return uof;
    }
    #endregion
}
