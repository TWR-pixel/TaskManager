using Microsoft.Extensions.Options;
using TaskManager.Application.Common.File;

namespace TaskManager.Infrastructure.File;

public sealed class FileReader(IOptions<FileWriterOptions> options) : IFileReader
{
    private readonly FileWriterOptions options = options.Value;

    public FileStream OpenRead(string imageName)
    {
        var stream = System.IO.File.OpenRead(options.PathForUserProfileImages + "\\" + imageName);

        return stream;
    }
}
