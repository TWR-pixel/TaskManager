using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Application.UserTask;

public static class UserTaskDtoMapperExtensions
{
    public static UserTaskDto ToResponse(this UserTaskEntity entity)
    {
        var dto = new UserTaskDto(entity.Title,
                                  entity.Description,
                                  entity.CreatedAt,
                                  entity.CompletedAt,
                                  entity.IsCompleted,
                                  entity.IsInProgress,
                                  entity.TaskColumn.Id);

        return dto;
    }

    public static IEnumerable<UserTaskDto> ToResponses(this IEnumerable<UserTaskEntity> entities)
    {
        var dtos = entities.Select(x => x.ToResponse());

        return dtos;
    }

}
