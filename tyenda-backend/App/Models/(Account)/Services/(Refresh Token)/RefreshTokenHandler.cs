using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Session_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Refresh_Token_
{
    public class RefreshTokenHandler : IRequestHandler<RefreshToken, object>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(RefreshToken request, CancellationToken cancellationToken)
        {
            try
            {
                var accToken = request.RefreshTokenForm.AccessToken;
                var refToken = request.RefreshTokenForm.RefreshToken;
                var accountId = _tokenService.GetTokenClaim(accToken, "AccountId");
                var role = _tokenService.GetTokenClaim(accToken, "Role");

                var session = await _context.Sessions.SingleOrDefaultAsync(session => 
                    session.AccessToken == accToken && session.RefreshToken == refToken && session.AccountId == Guid.Parse(accountId) && session.ExpiresIn >= DateTime.UtcNow,
                    cancellationToken);

                if (session == null)
                {
                    throw new UnauthorizedAccessException("Session is no longer valid");
                }
                
                await Task.FromResult(_context.Sessions.Remove(session));
                
                //Generate Tokens
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("AccountId", accountId),
                    new Claim("Role", role)
                };
                
                var account = await _context.Accounts.SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }
                var newAccessToken = _tokenService.GenerateAccessToken(claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken(claims);

                var newSession = new Session()
                {
                    Id = Guid.NewGuid(),
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresIn = _tokenService.GetTokenExpiryDate(newRefreshToken),
                    AccountId = account.Id
                };
                await _context.Sessions.AddAsync(newSession, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                return new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}