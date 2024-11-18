using TaskManager.Application.Common.Requests;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserBoard.Queries.GetAllById;

public sealed record GetAllUserBoardsByOwnerIdRequest : RequestBase<IEnumerable<UserBoardDto>>
{
    public required int OwnerId { get; set; }
}


public sealed class GetAllUserBoardsByOwnerIdRequestHandler(IUnitOfWork unitOfWork) : RequestHandlerBase<GetAllUserBoardsByOwnerIdRequest, IEnumerable<UserBoardDto>>(unitOfWork)
{
    public override async Task<IEnumerable<UserBoardDto>> Handle(GetAllUserBoardsByOwnerIdRequest request, CancellationToken cancellationToken)
    {
        var userBoardEntities = await UnitOfWork.UserBoards.GetAllByOwnerId(request.OwnerId, cancellationToken);

        if (userBoardEntities is null)
            return []; // return empty array

        var responses = userBoardEntities.Select(u => new UserBoardDto(u.Name, u.Description, u.CreationTime, u.Id));

        return responses;
    }
}