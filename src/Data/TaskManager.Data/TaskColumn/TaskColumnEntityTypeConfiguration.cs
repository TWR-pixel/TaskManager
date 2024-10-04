using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Data.TaskColumn;

public sealed class TaskColumnEntityTypeConfiguration : IEntityTypeConfiguration<TaskColumnEntity>
{
    public void Configure(EntityTypeBuilder<TaskColumnEntity> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

    }
}
