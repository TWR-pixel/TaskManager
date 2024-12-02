using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;

namespace TaskManager.Application.UserOrganization;

public record UserOrganizationResponse : ResponseBase
{
    [SetsRequiredMembers]
    public UserOrganizationResponse(string name, int ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

    public required string Name { get; set; }
    public required int OwnerId { get; set; }
}
