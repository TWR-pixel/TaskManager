using TaskManager.Application.Common.Requests;
using TaskManager.Core.Entities.Users;
using TaskManager.Data;

namespace TaskManager.Application.Role.Requests;

public sealed class AddAndSaveRoleRequest : RequestBase<AddAndSaveRoleResponse>
{
    public required string Name { get; set; }
}

public sealed class AddAndSaveRoleResponse : ResponseBase
{
    public required string Name { get; set; }
}

public sealed class AddAndSaveRoleRequestHandler
    : RequestHandlerBase<AddAndSaveRoleRequest, AddAndSaveRoleResponse>
{
    private readonly EfRepositoryBase<RoleEntity> _roleRepo;

    public AddAndSaveRoleRequestHandler(EfRepositoryBase<RoleEntity> roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public override async Task<AddAndSaveRoleResponse> Handle(AddAndSaveRoleRequest request, CancellationToken cancellationToken)
    {
        var role = new RoleEntity { Name = request.Name };

        await _roleRepo.AddAsync(role, cancellationToken);

        var response = new AddAndSaveRoleResponse { Name = request.Name };

        return response;
    }
}
