using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Infastructure;

public sealed class TaskManagerDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TaskColumnEntity> TaskColumns { get; set; }
    public DbSet<UserTaskEntity> UserTasks { get; set; }
    public DbSet<RoleEntity> UserRoles { get; set; }

    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) => Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

}
