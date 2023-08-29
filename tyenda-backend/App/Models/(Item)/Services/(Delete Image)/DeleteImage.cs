using MediatR;

namespace tyenda_backend.App.Models._Item_.Services._Delete_Image_
{
    public class DeleteImage : IRequest<bool>
    {
        public DeleteImage(string[] imageIds)
        {
            ImageIds = imageIds;
        }

        public string[] ImageIds { get; set; }
    }
}