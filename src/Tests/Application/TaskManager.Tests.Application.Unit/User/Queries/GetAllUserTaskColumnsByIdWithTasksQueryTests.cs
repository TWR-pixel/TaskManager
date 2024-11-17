using System.Diagnostics;
using TaskManager.Application.TaskColumn.Requests.GetAllUserTasksColumnsByIdRequest;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Users;
using TaskManager.Infrastructure.Sqlite.Common;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Application.Unit.User.Queries;

public class GetAllUserTaskColumnsByIdWithTasksQueryTests : TestInitializer
{
    [Fact]
    public async Task GetAllUserTaskColumnsByIdWithTasksQuery_ShouldReturnResponse()
    {
        // Arrange
        var testUserId = 1;
        var testUserTaskColumnWithTasksId = 1;
        var testRole = new RoleEntity("User") { Id = 2 };
        var testUserTaskColumnsWithTasks = new List<UserTaskColumnEntity>();
        var testUser = new UserEntity(testRole, "iejwjf@mail.ru", "iojweoi", "ajjijefpiowehash", "saltioejfwoief", true)
        {
            Id = testUserId,
        };
        testUserTaskColumnsWithTasks.Add(new UserTaskColumnEntity()
        {
            Id = testUserTaskColumnWithTasksId,
            Owner = testUser,
            Title = "Test title",
            CreatedAt = DateTime.UtcNow,
            Ordering = 5
        });
        testUser.TaskColumns = testUserTaskColumnsWithTasks;

        var dbContext = InitDbContext(DefaultOptions);
        var testRepository = new RepositoryBase<UserEntity>(dbContext);
        var uof = InitUOF(dbContext);
        var request = new GetAllUserTaskColumnsByIdWithTasksRequest() { UserId = testUserId };
        var handler = new GetAllUserTaskColumnsByIdWithTasksRequestHandler(uof);
        await testRepository.AddAsync(testUser);
        await uof.SaveChangesAsync();

        //Act
        var response = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(response);
        Assert.NotEmpty(response.TaskColumns);

        Debug.WriteLine(response.TaskColumns.Last());
        Debug.WriteLine(response.TaskColumns.First());
    }
}
