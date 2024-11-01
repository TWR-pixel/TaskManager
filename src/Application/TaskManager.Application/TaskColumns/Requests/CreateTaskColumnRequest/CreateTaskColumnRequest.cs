using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.UseCases.TaskColumns.Specifications;

namespace TaskManager.Application.TaskColumns.Requests.CreateTaskColumnRequest;

/// <summary>
/// Request for creating and saving user's task column in db
/// </summary>
/// <param name="UserId"></param>
/// <param name="Title"></param
/// <param name="Description"></param>
public sealed record CreateTaskColumnRequest(int UserId, string Title, string? Description) : RequestBase<UserTaskColumnDto>;

/// <summary>
/// Response for user
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
public sealed record CreateTaskColumnResponse(int Id, string Title, string? Description) : ResponseBase;

public sealed class CreateTaskColumnRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<CreateTaskColumnRequest, UserTaskColumnDto>(unitOfWork)
{
    public override async Task<UserTaskColumnDto> Handle(CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"User with id '{request.UserId}' not found");

        var taskColumnEntity = new TaskColumnEntity(userEntity, request.Title, request.Description);

        var queryResult = await UnitOfWork.UserTaskColumns.AddAsync(taskColumnEntity, cancellationToken);

        var response = queryResult.ToResponse();

        return response;
    }
}