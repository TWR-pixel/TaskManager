using TaskManager.Application.Common.Requests.Commands;
using TaskManager.Domain.Entities.Roles;
using TaskManager.Domain.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Role.Commands;

public sealed record CreateRoleCommand(string Name) : CommandBase<RoleDto>;

public sealed class CreateRoleCommandHandler(IUnitOfWork unitOfWork)
        : CommandHandlerBase<CreateRoleCommand, RoleDto>(unitOfWork)
{
    public override async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new RoleEntity(request.Name);

        await UnitOfWork.Roles.AddAsync(role, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        var response = role.ToResponse();

        return response;
    }
}
