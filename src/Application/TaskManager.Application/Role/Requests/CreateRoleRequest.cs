using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Role.Requests;

public sealed class CreateRoleRequest : RequestBase<CreateRoleResponse>
{
    public required string Name { get; set; }
}

public sealed class CreateRoleResponse : ResponseBase
{
    public required string Name { get; set; }
}

public sealed class CreateRoleRequestHandler
    : RequestHandlerBase<CreateRoleRequest, CreateRoleResponse>
{
    private readonly EfRepositoryBase<RoleEntity> _roleRepo;

    public CreateRoleRequestHandler(EfRepositoryBase<RoleEntity> roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public override async Task<CreateRoleResponse> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = new RoleEntity { Name = request.Name };

        await _roleRepo.AddAsync(role, cancellationToken);

        var response = new CreateRoleResponse { Name = request.Name };

        return response;
    }
}
