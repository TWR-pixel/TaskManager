using TaskManager.Application.Common.Requests;
using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Roles;
using TaskManager.Core.UseCases.Common.UnitOfWorks;

namespace TaskManager.Application.Role.Requests;

public sealed record CreateRoleRequest(string Name) : RequestBase<RoleDto>;

public sealed class CreateRoleRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<CreateRoleRequest, RoleDto>(unitOfWork)
{
    public override async Task<RoleDto> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = new RoleEntity(request.Name);

        await UnitOfWork.Roles.AddAsync(role, cancellationToken);

        var response = role.ToResponse();

        return response;
    }
}
