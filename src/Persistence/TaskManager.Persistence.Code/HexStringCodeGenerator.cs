using System.Security.Cryptography;
using TaskManager.Application.Common.Code;

namespace TaskManager.Persistence.Code;

public sealed class HexStringCodeGenerator : ICodeGenerator<string>
{
    public string GenerateCode(int length)
    {
        var code = RandomNumberGenerator.GetHexString(length);

        return code;
    }
}
