using MediatR;
using TyendaBackend.App.Models._Account_.Services._Forget_Password_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Forget_Password_
{
    public class ForgetPassword : IRequest<object>
    {
        public ForgetPassword(ForgetPasswordForm forgetPasswordForm)
        {
            ForgetPasswordForm = forgetPasswordForm;
        }

        public ForgetPasswordForm ForgetPasswordForm { get; set; }
    }
}