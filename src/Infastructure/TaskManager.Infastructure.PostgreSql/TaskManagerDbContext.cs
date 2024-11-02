﻿using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Infrastructure.PostgreSql;

public sealed class TaskManagerDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TaskColumnEntity> TaskColumns { get; set; }
    public DbSet<UserTaskEntity> UserTasks { get; set; }
    public DbSet<RoleEntity> UserRoles { get; set; }

    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
        : base(options) => Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleEntity>().HasData(new RoleEntity
        {
            Id = 1,
            Name = "User"
        });

        base.OnModelCreating(modelBuilder);
    }
}
