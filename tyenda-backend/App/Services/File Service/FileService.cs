using System;
using System.IO;
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
        public string UploadFile(IFormFile file, string folderType)
        {
            var original = _hostingEnvironment.ContentRootPath;//current project path
            var wwwroot = Path.Combine(original, "wwwroot");//combine it with /wwwroot

            if (!Directory.Exists(wwwroot))//if the path doesn’t exist, then create folder
            {
                Directory.CreateDirectory(wwwroot);
            }
            
            var fileDirectory = Path.Combine(wwwroot, folderType);//Combine root & desired path
            if (!Directory.Exists(fileDirectory))//create if it doesn’t exist
            {
                Directory.CreateDirectory(fileDirectory);
            }

            int uniqueUtcTime = (int)(DateTime.UtcNow.Ticks & 0xFFFFFFFF) * (-1);
            var fileName = uniqueUtcTime+"-"+file.FileName;
            var fileUrl = Path.Combine(fileDirectory, fileName);
            using (FileStream fileStream = new FileStream(fileUrl, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream); //Copy the file to the location: 
            }

            return folderType + "/" + fileName;

        }
    }
}