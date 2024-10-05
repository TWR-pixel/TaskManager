using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.Entities.Common;

public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }
}
