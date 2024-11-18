using Microsoft.EntityFrameworkCore;
using TaskManager.DALImplementation.Sqlite;
using TaskManager.DALImplementation.Sqlite.Common;
using TaskManager.Domain.Entities.Roles;

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

    public static RepositoryBase<RoleEntity> InitRepo(TaskManagerDbContext dbContext)
    {
        var repo = new RoleRepository(dbContext);

        return repo;
    }

    public static UnitOfWork InitUOF(TaskManagerDbContext dbContext)
    {
        var _testRepository = new RoleRepository(dbContext);

        var userTasksRepo = new UserTaskRepository(dbContext);
        var usersRepo = new UserRepository(dbContext);
        var userTaskColumnRepo = new UserTaskColumnRepository(dbContext);

        var uof = new UnitOfWork(dbContext, userTasksRepo, userTaskColumnRepo, _testRepository, usersRepo);

        return uof;
    }

}
