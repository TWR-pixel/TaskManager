﻿using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.Entities.TaskColumns;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Infrastructure.PostgreSql;

public sealed class TaskManagerDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserTaskColumnEntity> TaskColumns { get; set; }
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
