namespace TaskManager.Domain.Entities.Common.Exceptions;

public class EmailDoesntExistException(string email) : Exception($"'{email}' not found or doesnt exist")
{
}
