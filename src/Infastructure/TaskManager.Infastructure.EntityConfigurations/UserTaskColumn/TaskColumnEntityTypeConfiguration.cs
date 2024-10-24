﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Infastructure.EntityConfigurations.UserTaskColumn;

public sealed class TaskColumnEntityTypeConfiguration : IEntityTypeConfiguration<TaskColumnEntity>
{
    public void Configure(EntityTypeBuilder<TaskColumnEntity> builder)
    {
        builder.ToTable("task_columns");

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(t => t.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Description)
            .IsRequired(false)
            .HasColumnName("description")
            .HasMaxLength(int.MaxValue);

        builder.Property("OwnerId")
            .HasColumnName("owner_id")
            .IsRequired();
    }
}
