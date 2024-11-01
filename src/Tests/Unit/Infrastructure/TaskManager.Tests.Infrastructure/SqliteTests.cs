using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Infastructure.Sqlite;

namespace TaskManager.Tests.Infrastructure;

public class SqliteTests
{
    [Fact]
    public async Task ShouldReturnValue()
    {
        var role = new RoleEntity("User");
        var defaultUsers = new List<UserEntity>()
        {
            new() {
                Id = 1,
                EmailLogin = "eiowjfweoifj",
                PasswordHash = "weofjoweijfiowejwoijf",
                PasswordSalt = "ioejfoiwefiowjeoifjweo",
                RefreshToken = " oiwejfoiwejfiwejoifwejiofj",
                Role = role,
                Username = "TestUsername"
            }
        };


        var lnk = new LinkedList<int>();
        
        var options = new DbContextOptionsBuilder<TaskManagerDbContext>()
            .UseInMemoryDatabase("Tests").Options;
        
        var context = new TaskManagerDbContext(options);


    }

    public class TaskColumnsOrder
    {
        public required int Id { get; set; }
        public string? Order { get; set; }
    }
}
