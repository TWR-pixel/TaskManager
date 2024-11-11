using TaskManager.Domain.Entities.Roles;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class AddRangeAsyncTests : TestInitializer
{
    [Fact]
    public async Task ShouldAddEntitiesToRepository()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);
        var uof = InitUOF(dbContext);
        var roleEntities = new List<RoleEntity>
    {
        new() { Id = 2, Name = "Test Entity 1" },
        new() { Id = 3, Name = "Test Entity 2" }
    };

        await testRepository.AddRangeAsync(roleEntities);
        await uof.SaveChangesAsync();

        var numberOfEntities = 3;
        var numberOfEntitiesInRepo = await testRepository.CountAsync();
        var allRoles = await testRepository.ListAsync();

        Assert.Equal(numberOfEntities, numberOfEntitiesInRepo);
        Assert.Contains(roleEntities[0], allRoles);
        Assert.Contains(roleEntities[1], allRoles);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldThrowsArgumentExceptionWithKeyOne()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);
        var uof = InitUOF(dbContext);
        var roleEntities = new List<RoleEntity>
    {
        new() {Id = 1, Name = "Test Entity 1"}
    };

        await testRepository.AddRangeAsync(roleEntities);

        await Assert.ThrowsAsync<ArgumentException>(async () => await uof.SaveChangesAsync());
    }
}
