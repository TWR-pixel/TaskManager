using TaskManager.Core.Entities.Common.Entities;

namespace TaskManager.Core.Entities.Boards;

public sealed class BoardEntity : EntityBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required 
}
