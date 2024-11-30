using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.Entities.Common.Entities;

public interface IEntity
{
    [Key]
    public int Id { get; set; }
}
