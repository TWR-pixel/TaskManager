using TaskManager.Domain.Entities.Common.Exceptions;

namespace TaskManager.Domain.Entities.UserOrganization;

public sealed class UserOrganizationNotFoundException(int id) : NotFoundException($"User organization with id '{id}' not found");
