using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._My_Role_
{
    public class MyRoleHandler : IRequestHandler<MyRole, object>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public MyRoleHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(MyRole request, CancellationToken cancellationToken)
        {
            try
            {
                var role = _tokenService.GetHeaderTokenClaim("Role");
                return await Task.FromResult(new
                {
                    Role = role
                });
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}