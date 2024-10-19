using System.Runtime.InteropServices;

namespace TaskManager.PublicApi.Common.Wrappers;

public static class EnvironmentWrapper
{
    /// <summary>
    /// Returns enironment variable with checking OS
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string? GetEnvironmentVariable(string name)
    {
        string? variable;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            variable = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(variable))
            {
                variable = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
            }

        }
        else
        {
            variable = Environment.GetEnvironmentVariable(name);
        }


        return variable;
    }
}
