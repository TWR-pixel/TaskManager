using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.Entities.Common.Entities;

public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }
}
