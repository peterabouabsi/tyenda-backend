using MediatR;
using tyenda_backend.App.Models._Account_.Services._Refresh_Token_.Form;

namespace tyenda_backend.App.Models._Account_.Services._Refresh_Token_
{
    public class RefreshToken : IRequest<object>
    {
        public RefreshToken(RefreshTokenForm refreshTokenForm)
        {
            RefreshTokenForm = refreshTokenForm;
        }

        public RefreshTokenForm RefreshTokenForm { get; set; }
    }
}