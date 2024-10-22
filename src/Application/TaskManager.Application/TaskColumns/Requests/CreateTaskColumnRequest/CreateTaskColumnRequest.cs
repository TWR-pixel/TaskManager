using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Application.TaskColumns.Requests.CreateTaskColumnRequest;

/// <summary>
/// Request for creating and saving user's task column in db
/// </summary>
/// <param name="UserId"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
public sealed record CreateTaskColumnRequest(int UserId, string Title, string? Description) : RequestBase<CreateTaskColumnResponse>;

/// <summary>
/// Response for user
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
public sealed record CreateTaskColumnResponse(int Id, string Title, string? Description) : ResponseBase;

public sealed class CreateTaskColumnRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<CreateTaskColumnRequest, CreateTaskColumnResponse>(unitOfWork)
{
    public override async Task<CreateTaskColumnResponse> Handle(CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"User with id '{request.UserId}' not found");

        var entity = new TaskColumnEntity(userEntity, request.Title, request.Description);

        var queryResult = await UnitOfWork.UserTaskColumns.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateTaskColumnResponse(queryResult.Id, queryResult.Title, queryResult.Description);

        return response;
    }
}