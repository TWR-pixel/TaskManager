using TaskManager.DALImplementation.Sqlite.Common.Exceptions;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class SaveChangesAsyncTests : TestInitializer
{
    [Fact]
    public void ShouldThrowDoNotUseThisMethodException()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);

#pragma warning disable CS0618 // Type or member is obsolete
        Assert.ThrowsAsync<DontUseThisMethodException>(() => testRepository.SaveChangesAsync());
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
