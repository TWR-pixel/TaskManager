using TaskManager.Domain.Entities.UserOrganization;

namespace TaskManager.Application.UserOrganization;

public static class UserOrganizationMapperExtensions
{
    public static UserOrganizationResponse ToResponse(this UserOrganizationEntity entity)
    {
        var response = new UserOrganizationResponse(entity.Name, entity.Owner.Id);

        return response;
    }

    public static IEnumerable<UserOrganizationResponse> ToResponses(this IEnumerable<UserOrganizationEntity> entities)
    {
        var responses = entities.Select(e => e.ToResponse());

        return responses;
    }
}
