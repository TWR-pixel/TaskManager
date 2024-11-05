using TaskManager.Application.Common.Requests.Handlers;
using TaskManager.Core.Entities.Roles;

namespace TaskManager.Application.Role.Requests;

public sealed record CreateRoleRequest(string Name) : RequestBase<CreateRoleResponse>;
public sealed record CreateRoleResponse(string Name) : ResponseBase;

public sealed class CreateRoleRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<CreateRoleRequest, CreateRoleResponse>(unitOfWork)
{
    public override async Task<CreateRoleResponse> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = new RoleEntity(request.Name);

        await UnitOfWork.Roles.AddAsync(role, cancellationToken);

        var response = new CreateRoleResponse(role.Name);

        return response;
    }
}
