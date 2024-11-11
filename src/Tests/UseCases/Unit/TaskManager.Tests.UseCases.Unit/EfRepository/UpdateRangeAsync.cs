using TaskManager.Domain.Entities.Roles;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.EfRepository;

public class UpdateRangeAsync : TestInitializer
{
    [Fact]
    public async Task ShouldUpdateEntitiesNamesAndSaveChanges()
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

        roleEntities[0].Name = "Updated Test Entity 1";
        roleEntities[1].Name = "Updated Test Entity 2";
        await testRepository.UpdateRangeAsync(roleEntities);
        await uof.SaveChangesAsync();

        var updatedEntity1 = await testRepository.GetByIdAsync(roleEntities[0].Id);
#pragma warning disable CS8602 // Dereferencing a probable null reference.
        Assert.Equal("Updated Test Entity 1", updatedEntity1.Name);

        var updatedEntity2 = await testRepository.GetByIdAsync(roleEntities[1].Id);
        Assert.Equal("Updated Test Entity 2", updatedEntity2.Name);
#pragma warning restore CS8602 // Dereferencing a probable null reference.
    }
}
