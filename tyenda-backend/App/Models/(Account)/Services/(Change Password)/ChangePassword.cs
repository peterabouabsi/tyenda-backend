using MediatR;
using tyenda_backend.App.Models._Account_.Services._Change_Password_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Change_Password_
{
    public class ChangePassword : IRequest<bool>
    {
        public ChangePassword(ChangePasswordForm changePasswordForm)
        {
            ChangePasswordForm = changePasswordForm;
        }

        public ChangePasswordForm ChangePasswordForm { get; set; }
    }
}