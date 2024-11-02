﻿using Ardalis.Specification;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Core.UseCases.TaskColumns.Specifications;

public sealed class ReadTaskColumnsByIdWithOwnerSpec : SingleResultSpecification<TaskColumnEntity>
{
    public ReadTaskColumnsByIdWithOwnerSpec(int taskColumnId)
    {
        Query
            .AsNoTracking()
            .Where(t => t.Id == taskColumnId)
            .Include(t => t.Owner)
            .Include(t => t.TasksInColumn);
    }
}