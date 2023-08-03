using MediatR;
using tyenda_backend.App.Models._Account_.Services._Update_Profile_.Forms;

namespace tyenda_backend.App.Models._Account_.Services._Update_Profile_
{
    public class UpdateProfile : IRequest<object>
    {
        public UpdateProfile(UpdateProfileForm updateProfileForm)
        {
            UpdateProfileForm = updateProfileForm;
        }

        public UpdateProfileForm? UpdateProfileForm { get; set; }
    }
}