using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User;

public sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(u => u.LoginEmail)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(u => u.PasswordSalt)
            .IsRequired()
            .HasMaxLength(512);

    }
}
