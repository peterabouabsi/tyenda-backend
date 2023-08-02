using MediatR;
using tyenda_backend.App.Models._Account_.Services._Login_Google_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Login_Google_
{
    public class LoginGoogle : IRequest<object>
    {
        public LoginGoogle(LoginGoogleForm loginGoogleForm)
        {
            LoginGoogleForm = loginGoogleForm;
        }

        public LoginGoogleForm LoginGoogleForm { get; set; }
    }
}