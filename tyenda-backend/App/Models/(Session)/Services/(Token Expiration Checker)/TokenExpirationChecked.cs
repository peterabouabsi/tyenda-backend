using MediatR;
using tyenda_backend.App.Models._Session_.Services._Token_Expiration_Checker_.Form;
using tyenda_backend.App.Models._Session_.Views;

namespace tyenda_backend.App.Models._Session_.Services._Token_Expiration_Checker_
{
    public class TokenExpirationChecker : IRequest<TokenExpirationCheckerView>
    {
        public TokenExpirationChecker(TokenExpirationCheckerForm tokenExpirationCheckerForm)
        {
            TokenExpirationCheckerForm = tokenExpirationCheckerForm;
        }

        public TokenExpirationCheckerForm TokenExpirationCheckerForm { get; set; }
    }
}