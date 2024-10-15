﻿using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Core.Entities.TaskColumns.Specifications;

public sealed class GetAllTaskColumnsWithTasksByIdSpec : SingleResultSpecification<UserEntity>
{
    public GetAllTaskColumnsWithTasksByIdSpec(int userId)
    {
        Query
            .Where(u => u.Id == userId)
            .Include(u => u.TaskColumns)!
                .ThenInclude(t => t.TasksInColumn);

    }
}
