using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.TaskColumns.Specifications;

namespace TaskManager.Application.TaskColumns.Requests.GetAllUserTasksColumnsByIdRequest;

public sealed record GetAllUserTaskColumnsByIdRequest : RequestBase<GetAllUserTaskColumnsByIdResponse>
{
    public required int UserId { get; set; }
}

public sealed record GetAllUserTaskColumnsByIdResponse : ResponseBase
{
    public required int UserId { get; set; }
    public required string UserName { get; set; }

    public required IEnumerable<UserTasksColumnsResponse> UserTaskColumns { get; set; }

    public sealed record UserTasksColumnsResponse
    {
        [SetsRequiredMembers]
        public UserTasksColumnsResponse(int id, string name, string content)
        {
            Id = id;
            Name = name;
            Content = content;
        }

        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Content { get; set; }
    }
}

public sealed class GetAllUserTaskColumnsByIdRequestHandler
    : RequestHandlerBase<GetAllUserTaskColumnsByIdRequest, GetAllUserTaskColumnsByIdResponse>
{
    public GetAllUserTaskColumnsByIdRequestHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async override Task<GetAllUserTaskColumnsByIdResponse> Handle(GetAllUserTaskColumnsByIdRequest request,
                                                                                      CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.Users
            .SingleOrDefaultAsync(new GetAllTaskColumnsWithTasksByIdSpec(request.UserId), cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

        queryResult.TaskColumns ??= [];

        var response = new GetAllUserTaskColumnsByIdResponse
        {
            UserId = queryResult.Id,
            UserName = queryResult.Username,

            UserTaskColumns = queryResult.TaskColumns.Select(u =>
                new GetAllUserTaskColumnsByIdResponse.UserTasksColumnsResponse(
                    u.Id,
                    u.Name,
                    u.Description ?? "Empty"))
        };

        return response;
    }
}
