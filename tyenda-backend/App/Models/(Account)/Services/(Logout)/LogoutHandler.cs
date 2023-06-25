using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Account_.Services._Logout_
{
    public class LogoutHandler : IRequestHandler<Logout, bool>
    {
        private readonly TyendaContext _context;

        public LogoutHandler(TyendaContext context)
        {
            _context = context;
        }
        
        public async Task<bool> Handle(Logout request, CancellationToken cancellationToken)
        {
            try
            {
                var session = await _context.Sessions.SingleOrDefaultAsync(session => 
                    session.AccessToken == request.LogoutForm.AccessToken && session.RefreshToken == request.LogoutForm.RefreshToken, cancellationToken);

                if (session == null)
                {
                    throw new Exception("Session not found");
                }

                await Task.FromResult(_context.Sessions.Remove(session));
                await _context.SaveChangesAsync(cancellationToken);
                
                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}