namespace TaskManager.Infrastructure.Sqlite.Common.Exceptions;

public sealed class DoNotUseThisMethodException(string methodName) : Exception($"do not use {methodName}");
