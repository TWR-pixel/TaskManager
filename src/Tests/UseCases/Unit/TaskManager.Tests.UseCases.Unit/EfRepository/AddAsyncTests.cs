using TaskManager.Domain.Entities.Roles;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class AddAsyncTests : TestInitializer
{
    [Fact]
    public async Task ShouldAddEntityToRepository()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);
        var uof = InitUOF(dbContext);
        var roleEntity = new RoleEntity { Id = 2, Name = "Test Entity" };

        await testRepository.AddAsync(roleEntity);
        await uof.SaveChangesAsync();

        var allRoles = await testRepository.ListAsync();

        Assert.Contains(roleEntity, allRoles);
    }
}
