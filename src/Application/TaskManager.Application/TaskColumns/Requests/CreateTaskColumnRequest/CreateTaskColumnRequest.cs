using TaskManager.Application.Common;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.TaskColumns;

namespace TaskManager.Application.TaskColumns.Requests.CreateTaskColumnRequest;

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

public sealed class CreateTaskColumnRequestHandler : RequestHandlerBase<CreateTaskColumnRequest, CreateTaskColumnResponse>
{
    public CreateTaskColumnRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public override async Task<CreateTaskColumnResponse> Handle(CreateTaskColumnRequest request, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new EntityNotFoundException("user not found by id " + request.UserId);

        var entity = new TaskColumnEntity
        {
            Name = request.Name,
            Description = request.Description,
            Owner = userEntity
        };

        var queryResult = await UnitOfWork.UserTaskColumns.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);


        var response = new CreateTaskColumnResponse(queryResult.Id, queryResult.Name, queryResult.Description);

        return response;
    }
}