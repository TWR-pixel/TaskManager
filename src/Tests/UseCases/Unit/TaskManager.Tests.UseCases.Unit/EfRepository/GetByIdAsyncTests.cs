using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class GetByIdAsyncTests : TestInitializer
{
    [Fact]
    public async Task ShouldGetRoleByIdAsyncOne()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);
        var roleId = 1;

        var roleById = await testRepository.GetByIdAsync(roleId);

        Assert.NotNull(roleById);
    }
}
