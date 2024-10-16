﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Entities.Roles;

namespace TaskManager.Infastructure.EntityConfigurations.Role;

public sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("roles");

        builder.HasData(new List<RoleEntity>()
        {
            new(){Id = 1, Name = "User"}
        });

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(r => r.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(40);
    }
}
