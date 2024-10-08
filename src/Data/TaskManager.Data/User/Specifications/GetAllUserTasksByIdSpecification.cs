﻿using Ardalis.Specification;
using TaskManager.Core.Entities.Users;

namespace TaskManager.Data.User.Specifications;

public sealed class GetAllUserTasksByIdSpecification : SingleResultSpecification<UserEntity>
{
    /// <summary>
    /// Returns new user with his tasks
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="includeTasks"></param>
    /// <param name="includeTaskColumns"></param>
    public GetAllUserTasksByIdSpecification(int userId, bool includeTasks = false, bool includeTaskColumns = false)
    {
        Query.Where(t => t.Id == userId)
            .Include(t => t.Tasks);
    }
}
