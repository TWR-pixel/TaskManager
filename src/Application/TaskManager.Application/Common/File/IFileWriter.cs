using Microsoft.AspNetCore.Http;

namespace TaskManager.Application.Common.File;

public interface IFileWriter
{
    public FileStream WriteToFromFormFile(string fileName, IFormFile formFile);
}
