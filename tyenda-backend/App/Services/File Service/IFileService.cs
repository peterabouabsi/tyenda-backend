using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace tyenda_backend.App.Services.File_Service
{
    public interface IFileService
    {
        public abstract string UploadFile(IFormFile file, string folderType, string id);
    }
}