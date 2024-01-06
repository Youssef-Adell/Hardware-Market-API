namespace Core.Interfaces.IExternalServices;

public interface IFileService
{
    Task<string> SaveFile(string folderName, Byte[] file);
    void DeleteFile(string fileRelativePath);
}
