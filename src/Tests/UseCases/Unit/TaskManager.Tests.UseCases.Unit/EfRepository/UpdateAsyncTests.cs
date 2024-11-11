using TaskManager.Domain.Entities.Roles;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class UpdateAsyncTests : TestInitializer
{
    [Fact]
    public async Task ShouldUpdateEntityNameAndSaveChangesInUOW()
    {
        var options = InitOptions();
        var dbContext = InitDbContext(options);
        var testRepository = InitRepo(dbContext);
        var uof = InitUOF(dbContext);
        var roleEntity = new RoleEntity { Id = 2, Name = "Test Entity" };
        var newEntityName = "Updated Test Entity";

        await testRepository.AddAsync(roleEntity);
        await uof.SaveChangesAsync();

        roleEntity.Name = newEntityName;
        await testRepository.UpdateAsync(roleEntity);
        await uof.SaveChangesAsync();

        var updatedEntity = await testRepository.GetByIdAsync(roleEntity.Id);

#pragma warning disable CS8602 // Dereferencing a probable null reference.
        Assert.Equal(newEntityName, updatedEntity.Name);
#pragma warning restore CS8602 // Dereferencing a probable null reference.
    }
}
