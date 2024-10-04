using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Role.Requests;

public sealed class AddAndSaveRoleRequest : RequestBase<AddAndSAveRoleResponse>
{
    public required string Name { get; set; }
}

public sealed class AddAndSAveRoleResponse : ResponseBase
{
    public required string Name { get; set; }
}

public sealed class AddAndSaveRoleRequestHandler : RequestHandlerBase<AddAndSaveRoleRequest, AddAndSAveRoleResponse>
{
    private readonly EfRepositoryBase<RoleEntity> _roleRepo;

    public AddAndSaveRoleRequestHandler(EfRepositoryBase<RoleEntity> roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public override async Task<AddAndSAveRoleResponse> Handle(AddAndSaveRoleRequest request, CancellationToken cancellationToken)
    {
        var role = new RoleEntity { Name = request.Name };

        await _roleRepo.AddAsync(role, cancellationToken);

        var response = new AddAndSAveRoleResponse { Name = request.Name };

        return response;
    }
}
