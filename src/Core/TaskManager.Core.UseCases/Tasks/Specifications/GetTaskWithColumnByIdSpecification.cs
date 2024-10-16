﻿using Ardalis.Specification;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.UseCases.Tasks.Specifications;

public sealed class GetTaskWithColumnByIdSpecification : SingleResultSpecification<UserTaskEntity>
{
    public GetTaskWithColumnByIdSpecification(int taskId)
    {
        Query
            .Where(t => t.Id == taskId)
            .Include(t => t.TaskColumn);
    }
}
