using MediatR;
using tyenda_backend.App.Models._Account_.Services._Store_Signup_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Store_Signup_
{
    public class StoreSignup : IRequest<bool>
    {
        public StoreSignup(StoreSignupForm storeSignupForm)
        {
            StoreSignupForm = storeSignupForm;
        }

        public StoreSignupForm StoreSignupForm { get; set; }
    }
}