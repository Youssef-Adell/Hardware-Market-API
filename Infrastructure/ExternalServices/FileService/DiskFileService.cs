using Core.Interfaces.IExternalServices;

namespace Infrastructure.ExternalServices.FileService;

public class DiskFileService : IFileService
{
    public async Task<string> SaveFile(string folderName, string fileName, byte[] fileData)
    {
        fileName = $"{Path.GetRandomFileName()}_{fileName}";
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
        var fullPath = Path.Combine(folderPath, fileName);

        //create the folder if it is not exist
        if (!Path.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        //save the file in the specified folder
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.WriteAsync(fileData);
        }

        var relativePath = Path.Combine(folderName, fileName);
        return relativePath;
    }

}
