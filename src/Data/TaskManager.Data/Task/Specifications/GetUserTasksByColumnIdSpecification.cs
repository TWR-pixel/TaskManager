﻿using Ardalis.Specification;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Data.Task.Specifications;

public sealed class GetUserTasksByColumnIdSpecification : Specification<TaskColumnEntity>
{
    public GetUserTasksByColumnIdSpecification(int taskColumnId)
    {
        Query
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.TasksInColumn);
    }
}
