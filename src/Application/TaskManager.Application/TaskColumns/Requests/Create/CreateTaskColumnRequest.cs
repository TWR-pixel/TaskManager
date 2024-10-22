using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.TaskColumns;
using TaskManager.Core.Entities.Users.Exceptions;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.TaskColumns.Requests.Create;

/// <summary>
/// Request for creating and saving user's task column in db
/// </summary>
/// <param name="UserId"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
public sealed record CreateTaskColumnRequest(int UserId, string Name, string? Description) : RequestBase<CreateTaskColumnResponse>;

/// <summary>
/// Response for user
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
public sealed record CreateTaskColumnResponse(int Id, string Name, string? Description) : ResponseBase;

public sealed class CreateTaskColumnRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<CreateTaskColumnRequest, CreateTaskColumnResponse>(unitOfWork)
{
    public override async Task<CreateTaskColumnResponse> Handle(CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        var entity = new TaskColumnEntity(userEntity, request.Name, request.Description);

        var queryResult = await UnitOfWork.UserTaskColumns.AddAsync(entity, cancellationToken);
        
        var response = new CreateTaskColumnResponse(queryResult.Id, queryResult.Name, queryResult.Description);

        return response;
    }
}