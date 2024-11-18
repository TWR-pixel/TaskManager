using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TaskManager.Domain.Entities.Common.Entities;
using TaskManager.Domain.Entities.Tasks;
using TaskManager.Domain.Entities.Users;

namespace TaskManager.Domain.Entities.TaskColumns;

[Table("user_task_columns")]
[Index(nameof(CreatedAt), nameof(Title))]
public sealed class UserTaskColumnEntity : EntityBase
{
    [SetsRequiredMembers]
    public UserTaskColumnEntity(UserEntity owner, string title, int ordering, string? description = null)
    {
        Owner = owner;
        Title = title;
        Description = description;
        Ordering = ordering;
       // UserBoard = userBoard;
    }

    public UserTaskColumnEntity() { }

    [StringLength(128, MinimumLength = 3)]
    [Column("title")]
    public required string Title { get; set; }

    [StringLength(256)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("ordering")]
    public required int Ordering { get; set; }

    public IEnumerable<UserTaskEntity>? TasksInColumn { get; set; }

    [ForeignKey("owner_id")]
    public required UserEntity Owner { get; set; }

    //[ForeignKey("user_board_id")]
    //public required UserBoardEntity UserBoard { get; set; }
}
