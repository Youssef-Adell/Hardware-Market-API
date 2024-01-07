namespace Core.Interfaces.IExternalServices;

public interface IFileService
{
    Task<string> SaveFile(string folderName, byte[] file);
    void DeleteFile(string fileRelativePath);
    Task<bool> IsFileOfTypeImage(byte[] file);
    bool IsFileSizeExceedsLimit(byte[] file, int sizeLimit);
}
