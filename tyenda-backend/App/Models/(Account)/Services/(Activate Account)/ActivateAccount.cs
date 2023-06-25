using MediatR;
using tyenda_backend.App.Models._Account_.Services._Activate_Account_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Activate_Account_
{
    public class ActivateAccount : IRequest<bool>
    {
        public ActivateAccount(ActivateAccountForm activateAccountForm)
        {
            ActivateAccountForm = activateAccountForm;
        }

        public ActivateAccountForm ActivateAccountForm { get; set; }
    }
}