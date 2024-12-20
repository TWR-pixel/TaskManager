﻿using TaskManager.Application.UserTask;
using TaskManager.Application.UserTaskColumn;
using TaskManager.Domain.Entities.TaskColumns;

namespace TaskManager.Application.UserTaskColumn;

public static class UserTaskColumnMapperExtensions
{
    public static UserTaskColumnDto ToResponse(this UserTaskColumnEntity entity)
    {
        entity.TasksInColumn ??= [];

        var dto = new UserTaskColumnDto(entity.Title,
                                        entity.Description,
                                        entity.TasksInColumn.ToResponses(),
                                        entity.Owner.Id,
                                        entity.Id
                                        );

        return dto;
    }

    public static IEnumerable<UserTaskColumnDto> ToResponses(this IEnumerable<UserTaskColumnEntity> entities)
    {
        var dtos = entities.Select(x => x.ToResponse());

        return dtos;
    }
}
