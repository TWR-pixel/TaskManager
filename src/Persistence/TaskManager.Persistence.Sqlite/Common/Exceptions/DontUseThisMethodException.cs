namespace TaskManager.Persistence.Sqlite.Common.Exceptions;

public sealed class DontUseThisMethodException(string methodName) : Exception($"do not use {methodName}");
