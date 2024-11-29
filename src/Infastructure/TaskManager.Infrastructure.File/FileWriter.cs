using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TaskManager.Application.Common.File;

namespace TaskManager.Infrastructure.File;

public sealed class FileWriter(IOptions<FileWriterOptions> options) : IFileWriter
{
    private readonly FileWriterOptions options = options.Value;

    public FileStream WriteToFromFormFile(string fileName, IFormFile formFile)
    {
        using var stream = System.IO.File.Create(options.PathForUserProfileImages + "\\" + fileName);

        formFile.CopyTo(stream);

        return stream;
    }
}
