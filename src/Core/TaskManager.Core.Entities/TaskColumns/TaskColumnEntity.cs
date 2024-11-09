﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Domain.Entities.TaskColumns;

[Table("task_columns")]
public sealed class TaskColumnEntity : EntityBase
{
    [SetsRequiredMembers]
    public TaskColumnEntity(UserEntity owner, string title, string? description = null)
    {
        Owner = owner;
        Title = title;
        Description = description;
    }

    public TaskColumnEntity() { }

    [StringLength(128, MinimumLength = 3)]
    [Column("title")]
    public required string Title { get; set; }

    [StringLength(256)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public IEnumerable<UserTaskEntity>? TasksInColumn { get; set; }

    [ForeignKey("owner_id")]
    public required UserEntity Owner { get; set; }
}
