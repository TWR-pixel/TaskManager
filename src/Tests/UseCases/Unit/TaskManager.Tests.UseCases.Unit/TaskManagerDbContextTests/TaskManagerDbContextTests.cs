using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Infrastructure.Unit.TaskManagerDbContextTests;

public class TaskManagerDbContextTests : TestInitializer
{
    [Fact]
    public async Task ShouldContainsRoleWithNameUser()
    {
        //Arrange
        var dbContext = InitDbContext(DefaultOptions);
        var RoleNameUser = "User".ToUpper();

        //Act
#pragma warning disable CA1862 // Используйте перегрузки метода "StringComparison" для сравнения строк без учета регистра
        var userRoleEntity = await dbContext.Set<RoleEntity>().FirstOrDefaultAsync(r => r.Name.ToUpper() == RoleNameUser);
#pragma warning restore CA1862 // Используйте перегрузки метода "StringComparison" для сравнения строк без учета регистра

        //Assert
        Assert.NotNull(userRoleEntity);
    }
}
