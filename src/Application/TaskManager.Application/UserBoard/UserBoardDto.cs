using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.UserBoard;

public sealed record UserBoardDto
{
    [SetsRequiredMembers]
    public UserBoardDto(string name, string description, DateTime creationTime, int ownerId)
    {
        Name = name;
        Description = description;
        CreationTime = creationTime;
        OwnerId = ownerId;
    }

    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public required DateTime CreationTime { get; set; }
    public required int OwnerId { get; set; }
}
