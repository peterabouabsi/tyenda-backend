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
            var original = _hostingEnvironment.ContentRootPath;//current project path
            var wwwroot = Path.Combine(original, "wwwroot");//combine it with /wwwroot

            if (!Directory.Exists(wwwroot))//if the path doesn’t exist, then create folder
            {
                Directory.CreateDirectory(wwwroot);
            }
            
            var profilesDirectory = Path.Combine(wwwroot, folderType);//Combine root & desired path
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
    }
}