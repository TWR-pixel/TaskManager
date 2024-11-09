namespace TaskManager.Tests.UseCases.Unit;

internal static class AssertWrapper
{
    public static void DoesntThrow<T>(Action testCode) where T : Exception
    {
        try
        {
            testCode();
        }
        catch (T)
        {
            Assert.Fail();
        }
    }
}
