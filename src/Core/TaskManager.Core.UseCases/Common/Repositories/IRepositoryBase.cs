﻿using TaskManager.Domain.Entities.Common.Entities;

namespace TaskManager.Domain.UseCases.Common.Repositories;

public interface IRepositoryBase<TEntity> : IReadRepositoryBase<TEntity>, Ardalis.Specification.IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
}