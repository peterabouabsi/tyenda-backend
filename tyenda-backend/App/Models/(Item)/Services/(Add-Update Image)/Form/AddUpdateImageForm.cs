using Microsoft.AspNetCore.Http;

namespace tyenda_backend.App.Models._Item_.Services._Add_Update_Image_.Form
{
    public class AddUpdateImageForm
    {
        public IFormFile? File { get; set; }
        public string ItemId { get; set; } = "";
        public string? Id { get; set; } = "";
    }
}