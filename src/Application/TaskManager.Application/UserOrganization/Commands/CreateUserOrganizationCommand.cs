using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.UserOrganization;
using TaskManager.Domain.Entities.Users.Exceptions;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.UserOrganization.Commands;

public sealed record CreateUserOrganizationCommand : CommandBase<UserOrganizationResponse>
{
    public required string Name { get; set; }
    public required int OwnerId { get; set; }
}

public sealed class CreateUserOrganizationCommandHandler(IUnitOfWork unitOfWork) : CommandHandlerBase<CreateUserOrganizationCommand, UserOrganizationResponse>(unitOfWork)
{
    public override async Task<UserOrganizationResponse> Handle(CreateUserOrganizationCommand command, CancellationToken cancellationToken)
    {
        var ownerEntity = await UnitOfWork.Users.FindByIdAsync(command.OwnerId, cancellationToken)
            ?? throw new UserNotFoundException(command.OwnerId);

        var userOrganizationEntity = new UserOrganizationEntity(command.Name, ownerEntity);
        await UnitOfWork.UserOrganizations.AddAsync(userOrganizationEntity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = userOrganizationEntity.ToResponse();

        return response;
    }
}
