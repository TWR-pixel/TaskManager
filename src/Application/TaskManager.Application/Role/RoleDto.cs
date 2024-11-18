using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.Role;

public sealed record RoleDto
{
    public required string Name { get; set; }

    [SetsRequiredMembers]
    public RoleDto(string name)
    {
        Name = name;
    }
}
