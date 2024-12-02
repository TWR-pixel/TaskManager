using TaskManager.Domain.Entities.UserOrganization;
using TaskManager.Domain.UseCases.Common.Repositories;

namespace TaskManager.Domain.UseCases.UserOrganization;

public interface IUserOrganizationRepository : IRepository<UserOrganizationEntity>
{
    public Task<UserOrganizationEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
