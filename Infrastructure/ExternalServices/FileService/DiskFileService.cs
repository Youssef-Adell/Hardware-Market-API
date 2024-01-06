using Core.Interfaces.IExternalServices;

namespace Infrastructure.ExternalServices.FileService;

public class DiskFileService : IFileService
{
    public async Task<string> SaveFile(string folderName, string fileName, byte[] fileData)
    {
        //create random name and add to it to the extension of the file
        fileName = $"{Path.GetRandomFileName()}.{Path.GetExtension(fileName)}";

        //create the folder if it is not exist
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
        if (!Path.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        //save the file in the specified folder
        var fullPath = Path.Combine(folderPath, fileName);
        using (var fileStream = File.Create(fullPath))
        {
            await fileStream.WriteAsync(fileData);
        }

        var relativePath = Path.Combine(folderName, fileName);
        return relativePath;
    }

}
