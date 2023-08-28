using MediatR;
using Microsoft.AspNetCore.Http;

namespace tyenda_backend.App.Models._Account_.Services._Upload_Profile_
{
    public class UploadProfile : IRequest<string>
    {
        public UploadProfile(IFormFile file, string? folder)
        {
            File = file;
            Folder = folder;
        }

        public IFormFile File { get; set; }
        public string? Folder { get; set; }
    }
}