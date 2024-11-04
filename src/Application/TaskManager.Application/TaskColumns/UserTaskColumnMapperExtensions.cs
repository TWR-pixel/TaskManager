using TaskManager.Application.Tasks;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Application.TaskColumns;

public static class UserTaskColumnMapperExtensions
{
    public static UserTaskColumnDto ToResponse(this TaskColumnEntity entity)
    {
        entity.TasksInColumn ??= [];

        var dto = new UserTaskColumnDto(entity.Title, entity.Description, entity.TasksInColumn.ToResponses(), entity.Owner.Id, entity.Id);

        return dto;
    }

    public static IEnumerable<UserTaskColumnDto> ToResponses(this IEnumerable<TaskColumnEntity> entities)
    {
        var dtos = entities.Select(x => x.ToResponse());

        return dtos;
    }
}
