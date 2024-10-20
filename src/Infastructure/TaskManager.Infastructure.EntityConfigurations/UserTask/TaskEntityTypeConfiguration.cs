using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Infastructure.EntityConfigurations.UserTask;

public sealed class TaskEntityTypeConfiguration : IEntityTypeConfiguration<UserTaskEntity>
{
    public void Configure(EntityTypeBuilder<UserTaskEntity> builder)
    {
        builder.ToTable("tasks");

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(t => t.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Description)
            .HasColumnName("content")
            .IsRequired()
            .HasMaxLength(int.MaxValue);

        builder.Property(t => t.IsCompleted)
            .HasColumnName("is_completed")
            .IsRequired();

        builder.Property(t => t.IsInProgress)
            .HasColumnName("is_in_progress")
            .IsRequired();

        builder.Property("OwnerId")
            .HasColumnName("owner_id")
            .IsRequired();

        builder.Property("TaskColumnId")
            .HasColumnName("task_column_id")
            .IsRequired();
    }
}
