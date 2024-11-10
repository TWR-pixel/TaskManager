using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.UseCases.Roles.Specifications;
using TaskManager.Infrastructure.Sqlite.Common.Exceptions;

namespace TaskManager.Tests.Infrastructure.Unit;

public class EfRepositoryTests : TestInitializer
{
    public EfRepositoryTests() { }

    #region Tests
    [Fact]
    public async Task GetByIdAsync_ShouldGetRoleByIdAsync_One()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var roleId = 1;

        var roleById = await testRepository.GetByIdAsync(roleId);

        Assert.NotNull(roleById);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ShouldGetRoleByNameSpec_WithRoleName_User()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var roleName = "User";

        var roleByName = await testRepository.SingleOrDefaultAsync(new GetRoleByNameSpec(roleName));

        Assert.NotNull(roleByName);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityToRepository()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var uof = InitTestUnitOfWork(dbContext);
        var roleEntity = new RoleEntity { Id = 2, Name = "Test Entity" };

        await testRepository.AddAsync(roleEntity);
        await uof.SaveChangesAsync();

        var allRoles = await testRepository.ListAsync();

        Assert.Contains(roleEntity, allRoles);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddEntitiesToRepository()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var uof = InitTestUnitOfWork(dbContext);
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
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var uof = InitTestUnitOfWork(dbContext);
        var roleEntities = new List<RoleEntity>
        {
            new() {Id = 1, Name = "Test Entity 1"}
        };

        await testRepository.AddRangeAsync(roleEntities);

        await Assert.ThrowsAsync<ArgumentException>(async () => await uof.SaveChangesAsync());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntityFromDbContext()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var roleEntity = new RoleEntity { Id = 1, Name = "Test Entity" };
        await testRepository.AddAsync(roleEntity);

        await testRepository.DeleteAsync(roleEntity);
        var allRoles = await testRepository.ListAsync();

        Assert.DoesNotContain(roleEntity, allRoles);
    }

    [Fact]
    public async Task DeleteRangeAsync_ShouldRemoveEntitiesFromRepository()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var uof = InitTestUnitOfWork(dbContext);
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

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntityInRepository()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var uof = InitTestUnitOfWork(dbContext);
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

    [Fact]
    public async Task UpdateRangeAsync_ShouldUpdateEntitiesInRepository()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);
        var uof = InitTestUnitOfWork(dbContext);
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

    [Fact]
    public void SaveChangesAsync_ShouldThrowDoNotUseThisMethodException()
    {
        var options = InitTestOptions();
        var dbContext = InitTestDbContext(options);
        var testRepository = InitTestRepo(dbContext);

#pragma warning disable CS0618 // Type or member is obsolete
        Assert.ThrowsAsync<DoNotUseThisMethodException>(() => testRepository.SaveChangesAsync());
#pragma warning restore CS0618 // Type or member is obsolete
    }
    #endregion

}
