using System.Security.Cryptography;
using System.Text;
using TaskManager.Application.Common.File;

namespace TaskManager.Infrastructure.File;

public sealed class RandomFileNameGenerator : IRandomFileNameGenerator
{
    public string GenerateRandomFileName()
    {
        var fileName = Hash512(DateTime.Now + Path.GetRandomFileName());

        return fileName;
    }

    private static string Hash512(string value)
    {
        var inputBytes = Encoding.ASCII.GetBytes(value);
        var hash = SHA512.HashData(inputBytes);
        var sb = new StringBuilder();
        for (var i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
