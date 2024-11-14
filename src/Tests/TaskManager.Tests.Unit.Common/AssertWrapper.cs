using Xunit;

namespace TaskManager.Tests.Unit.Common;

public static class AssertWrapper
{
    public static void DoesntThrow<T>(Action testCode) where T : Exception
    {
        try
        {
            testCode();
        }
        catch (T)
        {
            Assert.Fail($"Error thrown with name {typeof(T).Name}");
        }
    }
}
