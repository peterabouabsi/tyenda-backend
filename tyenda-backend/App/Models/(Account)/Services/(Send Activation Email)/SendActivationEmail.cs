using MediatR;
using tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_
{
    public class SendActivationEmail : IRequest<bool>
    {
        public SendActivationEmail(SendActivationEmailForm sendActivationEmailForm)
        {
            SendActivationEmailForm = sendActivationEmailForm;
        }

        public SendActivationEmailForm SendActivationEmailForm { get; set; }
    }
}