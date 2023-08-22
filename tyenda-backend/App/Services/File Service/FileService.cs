using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace tyenda_backend.App.Services.File_Service
{
    
    public class FileService : IFileService
    {
    
        private readonly IHostEnvironment _hostingEnvironment;

        public FileService(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //folderType: Profiles, Backgrounds, Items
        public string UploadFile(IFormFile file, string folderType, string id)
        {

            var wwwroot = "wwwroot";
            var profilesDirectory = "Profiles";
            if (!Directory.Exists(wwwroot))//if the path doesn’t exist, then create folder
            {
                Directory.CreateDirectory(wwwroot);
            }
            
            if (!Directory.Exists(profilesDirectory))//create if it doesn’t exist
            {
                Directory.CreateDirectory(profilesDirectory);
            }

            var profileDirectory = Path.Combine(profilesDirectory, id);
            if (Directory.Exists(profileDirectory))
            {
                // Delete all files in the directory
                DirectoryInfo directoryInfo = new DirectoryInfo(profileDirectory);
                foreach (FileInfo innerFile in directoryInfo.GetFiles())
                {
                    innerFile.Delete();
                }
                
                Directory.Delete(profileDirectory);
            }
            
            //Create Directory /Profiles/{id}
            Directory.CreateDirectory(profileDirectory);

            //Upload file /Profiles/{id}/{fileName}
            var fileName = id+Path.GetExtension(file.FileName);
            var fileUrl = Path.Combine(profileDirectory, fileName);

            using (FileStream fileStream = new FileStream(fileUrl, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream); //Copy the file to the location: 
            }

            return folderType + "/" + id + "/" + fileName;
        }

        public string UploadItemImageFile(IFormFile file, string folderType, string imageId, string itemId)
        {
            var wwwroot = "wwwroot";
            var itemsSubdirectory = "Items";
    
            var itemsDirectory = Path.Combine(wwwroot, itemsSubdirectory);
            var itemDirectory = Path.Combine(itemsDirectory, itemId);
            
            if (!Directory.Exists(wwwroot))//if the path doesn’t exist, then create folder
            {
                Directory.CreateDirectory(wwwroot);
            }
            
            if (!Directory.Exists(itemsDirectory))//create if it doesn’t exist
            {
                Directory.CreateDirectory(itemsDirectory);
            }
            
            if (!Directory.Exists(itemDirectory))//create if it doesn’t exist
            {
                Directory.CreateDirectory(itemDirectory);
            }
            
            //Upload file /Images/{itemId}/{fileName}
            var fileName = imageId+Path.GetExtension(file.FileName);
            var fileUrl = Path.Combine(itemDirectory, fileName);
            
            if(!String.IsNullOrEmpty(imageId)){
                //Delete and Upload a new image
                try
                {
                    
                    FileInfo fileInfo = new FileInfo(fileUrl);
                    if (fileInfo.Exists)
                    {
                        // Ensure the file attributes allow modification
                        File.SetAttributes(fileUrl, FileAttributes.Normal);

                        try
                        {
                            File.Delete(fileUrl);
                            Console.WriteLine("File deleted successfully.");
                        }
                        catch (IOException deleteEx)
                        {
                            Console.WriteLine($"An error occurred while deleting the file: {deleteEx.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("File does not exist at the specified path.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            }

            using (FileStream fileStream = new FileStream(fileUrl, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream); //Copy the file to the location: 
            }

            return folderType + "/" + itemId + "/" + fileName;
            
        }
    }
}