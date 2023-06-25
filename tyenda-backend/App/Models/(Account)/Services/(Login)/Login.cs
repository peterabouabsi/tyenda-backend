using MediatR;
using TyendaBackend.App.Models._Account_.Services._Login_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Login_
{
    public class Login : IRequest<object>
    {
        public Login(LoginForm loginForm)
        {
            LoginForm = loginForm;
        }

        public LoginForm LoginForm { get; set; }
    }
}