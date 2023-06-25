using MediatR;
using tyenda_backend.App.Models._Account_.Services._Logout_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Logout_
{
    public class Logout : IRequest<bool>
    {
        public Logout(LogoutForm logoutForm)
        {
            LogoutForm = logoutForm;
        }

        public LogoutForm LogoutForm { get; set; }
    }
}