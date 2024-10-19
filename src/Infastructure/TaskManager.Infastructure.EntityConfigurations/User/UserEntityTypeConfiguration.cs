using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Infastructure.EntityConfigurations.User;

public sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(u => u.EmailLogin)
            .HasColumnName("email_login")
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(u => u.EmailLogin);

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(u => u.PasswordSalt)
            .HasColumnName("password_salt")
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(u => u.IsEmailConfirmed)
            .HasColumnName("is_email_confirmed")
            .IsRequired(true);

        builder.Property("RoleId")
            .HasColumnName("role_id")
            .IsRequired();
    }
}
