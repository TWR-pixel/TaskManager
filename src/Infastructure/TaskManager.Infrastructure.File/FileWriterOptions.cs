namespace TaskManager.Infrastructure.File;

public sealed class FileWriterOptions
{
    public required string PathForUserProfileImages { get; set; }
    public required IEnumerable<string> AvailableFileExtensions { get; set; }
}
