namespace Core.Interfaces.IExternalServices;

public interface IFileService
{
    Task<string> SaveFile(string folderName, string fileName, Byte[] fileData);
}
