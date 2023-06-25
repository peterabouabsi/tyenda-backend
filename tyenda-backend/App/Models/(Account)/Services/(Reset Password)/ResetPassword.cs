using MediatR;
using tyenda_backend.App.Models._Account_.Services._Reset_Password_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Reset_Password_
{
    public class ResetPassword : IRequest<bool>
    {
        public ResetPassword(ResetPasswordForm resetPasswordForm)
        {
            ResetPasswordForm = resetPasswordForm;
        }

        public ResetPasswordForm ResetPasswordForm { get; set; }
    }
}