using TaskManager.Domain.Entities.Roles;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class DeleteRangeAsyncTests : TestInitializer
{
    [Fact]
    public async Task ShouldRemoveEntitiesFromRepository()
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

        await testRepository.DeleteRangeAsync(roleEntities);
        await uof.SaveChangesAsync();

        var allRoles = await testRepository.ListAsync();

        Assert.DoesNotContain(roleEntities[0], allRoles);
        Assert.DoesNotContain(roleEntities[1], allRoles);
    }

}
