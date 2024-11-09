using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Infrastructure.Sqlite;
using TaskManager.Infrastructure.Sqlite.Common;

namespace TaskManager.Tests.UseCases.Unit;

public class EfRepositoryTests
{
    #region OperationCanceledException tests
    [Fact]
    public void ShouldUpdateAsyncDoesntThrowOperationCanceledException()
    {

    }

    [Fact]
    public void ShouldAddAsyncDoesntThrowOperationCanceledException()
    {
        var entity = new RoleEntity() { Id = 2, Name = "User2" };
        var ct = new CancellationToken();
        var roleRepo = InitDefaultRepository<RoleEntity>();

        AssertWrapper.DoesntThrow<OperationCanceledException>(async () =>
        {
            await roleRepo.AddAsync(entity, ct);
        });
    }
    #endregion

    [Fact]
    public async Task ShouldNotSaveChangesWhenAddAsync()
    {
        var roleRepo = InitDefaultRepository<RoleEntity>();
        var entity = new RoleEntity() { Id = 2, Name = "User2" };
        var ct = new CancellationToken();
        var beforeCount = await roleRepo.CountAsync();

        await roleRepo.AddAsync(entity, ct);

        var afterCount = await roleRepo.CountAsync();

        Assert.Equal(beforeCount, afterCount);
    }

    [Fact]
    public async Task ShouldDbContextHasRoleWithValueUser()
    {

    }

    #region Default initializers
    private DbContextOptions<TaskManagerDbContext> InitOptions()
    {
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseInMemoryDatabase("taskManager")
            .Options;

        return options;
    }

    private TaskManagerDbContext InitContext()
    {
        var dbContext = new TaskManagerDbContext(InitOptions());

        return dbContext;
    }

    private EfRepository<TEntity> InitDefaultRepository<TEntity>() where TEntity : EntityBase
    {
        var efRepository = new EfRepository<TEntity>(InitContext());

        return efRepository;
    }

    #endregion
}