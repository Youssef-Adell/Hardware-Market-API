using Core.Interfaces.IExternalServices;
using FileSignatures;

namespace Infrastructure.ExternalServices.FileService;

public class DiskFileService : IFileService
{
    private readonly IFileFormatInspector fileFormatInspector;
    private readonly string baseStoringPath;

    public DiskFileService(IFileFormatInspector fileFormatInspector)
    {
        this.fileFormatInspector = fileFormatInspector;
        baseStoringPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    }

    public async Task<string> SaveFile(string folderName, byte[] file)
    {
        //get the actual extension of the file (note that we dont care about filename or the extension provided with it because it may be faked)
        string? fileExtension;
        using (var memoryStream = new MemoryStream())
        {
            await memoryStream.WriteAsync(file);
            fileExtension = fileFormatInspector.DetermineFileFormat(memoryStream)?.Extension;
        }

        //Create random file name and add the extension to it
        var fileName = $"{Path.GetRandomFileName()}.{fileExtension}";

        //create the specified folder if it is not exist
        var folderPath = Path.Combine(baseStoringPath, folderName);
        if (!Path.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        //save the file in the specified folder
        var fullPath = Path.Combine(baseStoringPath, folderName, fileName);
        using (var fileStream = File.Create(fullPath))
        {
            await fileStream.WriteAsync(file);
        }

        var relativePath = Path.Combine(folderName, fileName);
        return relativePath;
    }

    public void DeleteFile(string fileRelativePath)
    {
        var fullPath = Path.Combine(baseStoringPath, fileRelativePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

}
