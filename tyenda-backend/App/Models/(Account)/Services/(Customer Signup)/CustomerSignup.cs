using MediatR;
using tyenda_backend.App.Models._Account_.Services._Customer_Signup_.Forms;

namespace tyenda_backend.App.Models._Account_.Services._Customer_Signup_
{
    public class CustomerSignup : IRequest<bool>
    {
        public CustomerSignup(CustomerSignupForm customerSignupForm)
        {
            CustomerSignupForm = customerSignupForm;
        }

        public CustomerSignupForm CustomerSignupForm { get; set; }
    }
}