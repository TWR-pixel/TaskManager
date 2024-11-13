namespace TaskManager.Domain.Entities.Common.Exceptions;

public class MxNotFoundException(string email) : Exception($"'{email}' not found or doesnt exist")
{
}
