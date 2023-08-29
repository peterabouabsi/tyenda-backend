using MediatR;
using Microsoft.AspNetCore.Http;

namespace tyenda_backend.App.Models._Store_.Services._Upload_Video_
{
    public class UploadVideo : IRequest<string>
    {
        public UploadVideo(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; set; }
    }
}