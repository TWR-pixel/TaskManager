using TaskManager.Application.Common.Requests.Queries;
using TaskManager.Domain.Entities.UserOrganization;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserOrganization.Queries;

public sealed record GetUserOrganizationByIdQuery : QueryBase<UserOrganizationResponse>
{
    public required int Id { get; set; }
}

public sealed class GetUserOrganizationByIdQueryHandler(IReadonlyUnitOfWork unitOfWork) : QueryHandlerBase<GetUserOrganizationByIdQuery, UserOrganizationResponse>(unitOfWork)
{
    public override async Task<UserOrganizationResponse> Handle(GetUserOrganizationByIdQuery query, CancellationToken cancellationToken)
    {
        var userOrganizationEntity = await UnitOfWork.UserOrganizations.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new UserOrganizationNotFoundException(query.Id);

        var response = userOrganizationEntity.ToResponse();

        return response;
    }
}
