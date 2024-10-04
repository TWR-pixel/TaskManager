using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Data.Task;

public sealed class TaskEntityTypeConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.ContentBody)
            .IsRequired()
            .HasMaxLength(int.MaxValue);

    }
}
