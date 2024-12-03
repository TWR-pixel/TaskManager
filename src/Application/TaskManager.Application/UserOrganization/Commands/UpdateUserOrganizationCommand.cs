using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.UserOrganization;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserOrganization.Commands;

public sealed record UpdateUserOrganizationCommand : CommandBase<UserOrganizationResponse>
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}

public sealed class UpdateUserOrganizationCommandHandler(IUnitOfWork unitOfWork) : CommandHandlerBase<UpdateUserOrganizationCommand, UserOrganizationResponse>(unitOfWork)
{
    public override async Task<UserOrganizationResponse> Handle(UpdateUserOrganizationCommand request, CancellationToken cancellationToken)
    {
        var userOrganizationEntity = await UnitOfWork.UserOrganizations.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new UserOrganizationNotFoundException(request.Id);

        userOrganizationEntity.Name = request.Name;

        await UnitOfWork.UserOrganizations.UpdateAsync(userOrganizationEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return userOrganizationEntity.ToResponse();
    }
}
