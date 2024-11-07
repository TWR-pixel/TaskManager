﻿using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Users.Specifications;

namespace TaskManager.Application.User.Queries.GetById;

public sealed record GetUserByIdRequest(int UserId) : QueryRequestBase<UserDto>;

public sealed class GetUserByIdRequestHandler(IReadUnitOfWork unitOfWork) : QueryRequestHandlerBase<GetUserByIdRequest, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new ReadUserByIdSpec(request.UserId), cancellationToken)
                ?? throw new UserNotFoundException(request.UserId);

        var response = queryResult.ToResponse();

        return response;
    }
}