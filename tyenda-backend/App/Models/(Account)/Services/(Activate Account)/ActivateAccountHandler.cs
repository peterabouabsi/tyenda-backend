using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Activate_Account_
{
    public class ActivateAccountHandler : IRequestHandler<ActivateAccount, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public ActivateAccountHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(ActivateAccount request, CancellationToken cancellationToken)
        {
            try
            {
                var existingToken = await _context.Tokens.SingleOrDefaultAsync(token =>
                    token.Value == request.ActivateAccountForm.Token
                    ,cancellationToken);

                if (existingToken == null)
                {
                    throw new Exception("Token is no longer valid");
                }

                var accountId = _tokenService.GetTokenClaim(request.ActivateAccountForm.Token, "AccountId");
                if (Guid.Parse(accountId) != existingToken.AccountId || existingToken.ExpiresIn <= DateTime.UtcNow)
                {
                    await Task.FromResult(_context.Tokens.Remove(existingToken));
                    await _context.SaveChangesAsync(cancellationToken);
                    throw new Exception("Token is no longer valid");
                }

                var account = await _context.Accounts.SingleOrDefaultAsync(account => 
                    account.Id == Guid.Parse(accountId),
                    cancellationToken);

                if (account == null)
                {
                    await Task.FromResult(_context.Tokens.Remove(existingToken));
                    await _context.SaveChangesAsync(cancellationToken);
                    throw new Exception("Account not found");
                }

                account.Active = true;
                account.CreatedAt = account.CreatedAt.ToUniversalTime(); 
                await Task.FromResult(_context.Accounts.Update(account));
                await Task.FromResult(_context.Tokens.Remove(existingToken));

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