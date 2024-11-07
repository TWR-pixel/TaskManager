﻿using MediatR;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Common.Requests.Handlers;

/// <summary>
/// Базовый обработчик для всех запросов
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class RequestHandlerBase<TRequest, TResponse>(IUnitOfWork unitOfWork) : IRequestHandler<TRequest, TResponse>
    where TRequest : RequestBase<TResponse>
    where TResponse : class
{
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}