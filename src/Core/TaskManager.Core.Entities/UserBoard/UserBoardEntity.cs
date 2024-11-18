using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.Entities.UserBoard;

[Table("user_boards")]
[Index(nameof(CreationTime))]
public sealed class UserBoardEntity : EntityBase
{
    public UserBoardEntity(string name, string description, DateTime creationTime)//, UserEntity owner)
    {
        Name = name;
        Description = description;
        CreationTime = creationTime;
       // Owner = owner;
    }

    public UserBoardEntity() { }

    [Column("name")]
    public required string Name { get; set; }

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("creation_time")]
    public required DateTime CreationTime { get; set; }

    //[ForeignKey("owner_id")]
    //public required UserEntity Owner { get; set; }

    //public IEnumerable<UserTaskColumnEntity>? UserTaskColumns { get; set; }
}
