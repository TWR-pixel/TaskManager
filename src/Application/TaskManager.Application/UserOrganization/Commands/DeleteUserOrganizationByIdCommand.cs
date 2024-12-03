using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.UserOrganization;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserOrganization.Commands;

public sealed class DeleteUserOrganizationByIdCommandHandler(IUnitOfWork unitOfWork) : CommandHandlerBase<DeleteByIdCommandBase<UserOrganizationResponse>, UserOrganizationResponse>(unitOfWork)
{
    public override async Task<UserOrganizationResponse> Handle(DeleteByIdCommandBase<UserOrganizationResponse> request, CancellationToken cancellationToken)
    {
        var userOrganizationEntity = await UnitOfWork.UserOrganizations.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new UserOrganizationNotFoundException(request.Id);

        await UnitOfWork.UserOrganizations.DeleteAsync(userOrganizationEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return userOrganizationEntity.ToResponse();
    }
}
