using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Reset_Password_
{
    public class ResetPasswordHandler : IRequestHandler<ResetPassword, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public ResetPasswordHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(ResetPassword request, CancellationToken cancellationToken)
        {
            try
            {
                var newPassword = request.ResetPasswordForm.NewPassword;
                var confirmPassword = request.ResetPasswordForm.ConfirmPassword;
                var token = request.ResetPasswordForm.Token;
                
                if (newPassword != confirmPassword)
                {
                    throw new Exception("Password and confirm password do not match.");
                }
                
                var existingToken = await _context.Tokens.SingleOrDefaultAsync(
                    prop => prop.Value == token
                    ,cancellationToken);

                if (existingToken == null)
                {
                    throw new Exception("Sorry, the link is no longer valid.");
                }

                var accountId = _tokenService.GetTokenClaim(token, "AccountId");
                if (Guid.Parse(accountId) != existingToken.AccountId)
                {
                    throw new Exception("Invalid or expired token");
                }

                
                //Reset password
                var account = await _context.Accounts
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
             
                var pwdMatch = BCrypt.Net.BCrypt.Verify(newPassword, account!.Password);
                if (pwdMatch)
                {
                    throw new Exception("New password must differ from current.");
                }

                account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                account.CreatedAt = DateTime.SpecifyKind(account.CreatedAt, DateTimeKind.Utc);
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