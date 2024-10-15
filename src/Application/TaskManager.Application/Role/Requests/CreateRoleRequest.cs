using System.Diagnostics.CodeAnalysis;
using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Common.UnitOfWorks;
using TaskManager.Core.Entities.Roles;
namespace TaskManager.Application.Role.Requests;

public sealed record CreateRoleRequest : RequestBase<CreateRoleResponse>
{
    public required string Name { get; set; }
}

public sealed record CreateRoleResponse : ResponseBase
{
    [SetsRequiredMembers]
    public CreateRoleResponse(string name)
    {
        Name = name;
    }

    public required string Name { get; set; }
}

public sealed class CreateRoleRequestHandler(IUnitOfWork unitOfWork)
        : RequestHandlerBase<CreateRoleRequest, CreateRoleResponse>(unitOfWork)
{
    public override async Task<CreateRoleResponse> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = new RoleEntity(request.Name);

        await UnitOfWork.Roles.AddAsync(role, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateRoleResponse(role.Name);

        return response;
    }
}
