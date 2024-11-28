namespace TaskManager.Application.Common.File;

public interface IFileReader
{
    public FileStream OpenRead(string imageName);
}
