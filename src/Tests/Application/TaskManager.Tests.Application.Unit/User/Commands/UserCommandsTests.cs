using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.User.Commands.DeleteById;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.Users;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Infrastructure.Sqlite.Common;
using TaskManager.Tests.Unit.Common;

namespace TaskManager.Tests.Application.Unit.User.Commands
{
    public class UserCommandsTests : TestInitializer
    {
        private readonly ServiceCollection services;
        private readonly ServiceProvider provider;

        public UserCommandsTests()
        {
            services = new ServiceCollection();
            provider = services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DeleteUserByIdRequest).Assembly))
                .BuildServiceProvider();
        }

        [Fact]
        public async void DeleteUserByIdCommand_ShouldThrowsUserNotFoundException()
        {
            // Arrange
            var dbContext = InitDbContext(DefaultOptions);
            var testRepository = InitRepo(dbContext);
            var uof = InitUOF(dbContext);
            var request = new DeleteUserByIdRequest(1);
            var handler = new DeleteUserByIdRequestHandler(uof);

            //Act Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteUserByIdCommand_ShouldReturnUserDto()
        {
            // Arrange
            var testUserId = 1;
            var testRole = new RoleEntity("User");
            var testUser = new UserEntity(testRole, "iejwjf@mail.ru", "iojweoi", "ajjijefpiowehash", "saltioejfwoief", true) { Id = testUserId };
            var dbContext = InitDbContext(DefaultOptions);
            var testRepository = new RepositoryBase<UserEntity>(dbContext);
            var uof = InitUOF(dbContext);
            var request = new DeleteUserByIdRequest(testUserId);
            var handler = new DeleteUserByIdRequestHandler(uof);

            //Act
            await testRepository.AddAsync(testUser);
            await uof.SaveChangesAsync();
            var response = await handler.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
        }


    }
}