using TaskManager.Domain.Entities.Roles;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class DeleteAsyncTests : TestInitializer
{
    [Fact]
    public async Task ShouldRemoveEntityFromDbContext()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);
        var roleEntity = new RoleEntity { Id = 1, Name = "Test Entity" };
        await testRepository.AddAsync(roleEntity);

        await testRepository.DeleteAsync(roleEntity);
        var allRoles = await testRepository.ListAsync();

        Assert.DoesNotContain(roleEntity, allRoles);
    }
}
