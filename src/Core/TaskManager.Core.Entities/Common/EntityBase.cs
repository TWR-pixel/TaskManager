using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.Entities.Common;

public abstract record EntityBase
{
    [Key]
    public required int Id { get; set; }
    public bool IsSoftDeleted { get; set; } = false;
}
