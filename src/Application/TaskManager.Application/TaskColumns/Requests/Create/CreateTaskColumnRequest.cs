using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users.Exceptions;

namespace TaskManager.Application.TaskColumns.Requests.Create;

/// <summary>
/// Request for creating and saving user's task column in db
/// </summary>
/// <param name="UserId"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
public sealed record CreateTaskColumnRequest(int UserId, string Name, string? Description) : RequestBase<UserTaskColumnDto>;

public sealed class CreateTaskColumnRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<CreateTaskColumnRequest, UserTaskColumnDto>(unitOfWork)
{
    public override async Task<UserTaskColumnDto> Handle(CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        var entity = new TaskColumnEntity(userEntity, request.Name, request.Description);

        var queryResult = await UnitOfWork.UserTaskColumns.AddAsync(entity, cancellationToken);

        var response = queryResult.ToResponse();

        return response;
    }
}