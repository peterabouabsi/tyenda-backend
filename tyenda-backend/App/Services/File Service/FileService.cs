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
        public string UploadFile(IFormFile file, string folderName, string id)
        {
            var wwwroot = "wwwroot"; //Root Directory -> /wwwrooot
            if (!Directory.Exists(wwwroot))//if the path doesn’t exist, then create folder
            {
                Directory.CreateDirectory(wwwroot);
            }
            
            var profilesDirectory = Path.Combine(wwwroot, folderName); //Profiles Directory -> /wwwroot/Profiles
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
                
                //Then remove directory
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

            return folderName + "/" + id + "/" + fileName;
        }

        public void DeleteItemDirectory(string id)
        {
            try
            {
                var wwwroot = "wwwroot"; //Root Directory -> /wwwrooot
                var itemsDirectory = Path.Combine(wwwroot, "Items"); //Folder Directory -> /wwwroot/{FOLDER}
                var itemDirectory = Path.Combine(itemsDirectory, id);
                if (Directory.Exists(itemDirectory))
                {
                    // Delete all files in the directory
                    DirectoryInfo directoryInfo = new DirectoryInfo(itemDirectory);
                    foreach (FileInfo innerFile in directoryInfo.GetFiles())
                    {
                        innerFile.Delete();
                    }
                    
                    //Then Remove directory
                    Directory.Delete(itemDirectory);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public string UploadItemImageFile(IFormFile file, string folderName, string imageId, string itemId)
        {
            var wwwroot = "wwwroot";
    
            var itemsDirectory = Path.Combine(wwwroot, folderName);
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

            using (FileStream fileStream = new FileStream(fileUrl, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream); //Copy the file to the location: 
            }

            return folderName + "/" + itemId + "/" + fileName;
            
        }
    }
}