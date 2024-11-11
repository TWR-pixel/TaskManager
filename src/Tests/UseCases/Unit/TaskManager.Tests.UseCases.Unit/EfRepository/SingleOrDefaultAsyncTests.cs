using TaskManager.Domain.UseCases.Roles.Specifications;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class SingleOrDefaultAsyncTests : TestInitializer
{
    [Fact]
    public async Task ShouldGetRoleByNameSpecWithRoleNameUser()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);
        var roleName = "User";

        var roleByName = await testRepository.SingleOrDefaultAsync(new GetRoleByNameSpec(roleName));

        Assert.NotNull(roleByName);
    }
}