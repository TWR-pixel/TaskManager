using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.User.Queries;

public sealed record GetAllUserOrganizationsQuery : QueryBase<UserDto>
{
    public required int Id { get; set; }
}

public sealed class GetAllUserOrganizationsQueryHandler(IReadonlyUnitOfWork unitOfWork) : QueryHandlerBase<GetAllUserOrganizationsQuery, UserDto>(unitOfWork)
{
    public override async Task<UserDto> Handle(GetAllUserOrganizationsQuery query, CancellationToken cancellationToken)
    {
        var userEntity = await UnitOfWork.Users.GetAllUserOrgranizations(query.Id, cancellationToken)
            ?? throw new UserNotFoundException(query.Id);

        var response = userEntity.ToResponse();

        return response;
    }
}
