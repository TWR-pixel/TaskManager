using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities.UserOrganization;
using TaskManager.Domain.UseCases.UserOrganization;
using TaskManager.Persistence.Sqlite.Common;

namespace TaskManager.Persistence.Sqlite.UserOrganization;

public class UserOrgranizationRepository(TaskManagerDbContext dbContext) : RepositoryBase<UserOrganizationEntity>(dbContext), IUserOrganizationRepository
{
    public async Task<UserOrganizationEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var userOrgranization = await DbContext.UserOrganizations
            .Include(u => u.Owner)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

        return userOrgranization;
    }
}
