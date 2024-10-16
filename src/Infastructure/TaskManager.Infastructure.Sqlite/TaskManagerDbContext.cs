using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;
using TaskManager.Infastructure.EntityConfigurations.Role;
using TaskManager.Infastructure.EntityConfigurations.User;
using TaskManager.Infastructure.EntityConfigurations.UserTask;
using TaskManager.Infastructure.EntityConfigurations.UserTaskColumn;

namespace TaskManager.Infastructure.Sqlite;

public sealed class TaskManagerDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TaskColumnEntity> TaskColumns { get; set; }
    public DbSet<UserTaskEntity> UserTasks { get; set; }
    public DbSet<RoleEntity> UserRoles { get; set; }

    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) 
        => Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TaskEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TaskColumnEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
