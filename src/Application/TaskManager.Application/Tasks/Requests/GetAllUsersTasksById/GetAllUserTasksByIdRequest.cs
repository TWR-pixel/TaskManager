using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.Exceptions;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.UseCases.Tasks.Specifications;

namespace TaskManager.Application.Tasks.Requests.GetAllUsersTasksById;

/// <summary>
/// Returns all user's tasks by id in database
/// </summary>
public sealed record GetAllUserTasksByIdRequest : RequestBase<GetAllUserTasksByIdResponse>
{
    public required int UserId { get; init; }
}

public sealed record GetAllUserTasksByIdResponse : ResponseBase
{
    public required int UserId { get; init; }
    public required string UserName { get; set; }
    public required string UserEmail { get; set; }

    public required IEnumerable<UserTaskByIdResponse> UserTasks { get; set; }

    public sealed class UserTaskByIdResponse
    {
        [SetsRequiredMembers]
        public UserTaskByIdResponse(string title,
                                    string content,
                                    bool isCompleted,
                                    bool isInProgress,
                                    DateTime createdAt,
                                    DateOnly? doTo,
                                    int id)
        {
            Title = title;
            Content = content;
            IsCompleted = isCompleted;
            IsInProgress = isInProgress;
            CreatedAt = createdAt;
            DoTo = doTo;
            Id = id;
        }

        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        public required bool IsCompleted { get; set; }
        public required bool IsInProgress { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateOnly? DoTo { get; set; }

    }
}

public sealed class GetAllUserTasksByIdRequestHandler(IUnitOfWork unitOfWork)
    : RequestHandlerBase<GetAllUserTasksByIdRequest, GetAllUserTasksByIdResponse>(unitOfWork)
{
    public override async Task<GetAllUserTasksByIdResponse> Handle(GetAllUserTasksByIdRequest request, CancellationToken cancellationToken)
    {
        var userQueryResult = await UnitOfWork.Users.SingleOrDefaultAsync(new ReadAllUserTasksByIdSpecification(request.UserId), cancellationToken)
            ?? throw new EntityNotFoundException($"User with id {request.UserId} not found");

        userQueryResult.Tasks ??= [];

        var response = new GetAllUserTasksByIdResponse
        {
            UserId = request.UserId,
            UserName = userQueryResult.Username,
            UserEmail = userQueryResult.EmailLogin,

            UserTasks = userQueryResult.Tasks.Select(static t => new GetAllUserTasksByIdResponse.UserTaskByIdResponse(
                t.Title,
                t.Content,
                t.IsCompleted,
                t.IsInProgress,
                t.CreatedAt,
                t.DoTo,
                t.Id))
        };

        return response;
    }
}
