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
        var formFileExtension = Path.GetExtension(formFile.FileName);

        if (string.IsNullOrWhiteSpace(formFileExtension))
            throw new NotSupportedException(nameof(formFileExtension));

        var availableFileExtension = options.AvailableFileExtensions.FirstOrDefault(e => "." + e.ToUpper() == formFileExtension.ToUpper());

        if (availableFileExtension is null)
            throw new NotSupportedException(nameof(availableFileExtension));

        formFile.CopyTo(stream);

        return stream;
    }
}
