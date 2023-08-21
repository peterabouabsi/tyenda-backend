using MediatR;
using Microsoft.AspNetCore.Http;
using tyenda_backend.App.Models._Item_.Services._Add_Update_Image_.Form;
using tyenda_backend.App.Models._ItemImage_;

namespace tyenda_backend.App.Models._Item_.Services._Add_Update_Image_
{
    public class AddUpdateImage : IRequest<ItemImage>
    {
        public AddUpdateImage(AddUpdateImageForm addUpdateImageForm)
        {
            AddUpdateImageForm = addUpdateImageForm;
        }

        public AddUpdateImageForm AddUpdateImageForm { get; set; }
    }
}