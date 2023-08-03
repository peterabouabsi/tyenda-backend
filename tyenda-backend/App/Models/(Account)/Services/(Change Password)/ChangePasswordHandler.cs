using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Change_Password_
{
    public class ChangePasswordHandler : IRequestHandler<ChangePassword, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        
        public ChangePasswordHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim("AccountId");

                var account = await _context.Accounts.SingleOrDefaultAsync(account => 
                    account.Id == Guid.Parse(accountId), cancellationToken);

                if (account == null)
                {
                    throw new Exception("Account not found");
                }
                
                if(account.Password != null){
                    var pwdMatch = BCrypt.Net.BCrypt.Verify(request.ChangePasswordForm.OldPassword, account.Password);
                    if (pwdMatch == false)
                    {
                        throw new Exception("Incorrect old password entered. Please ensure that you have entered your current password correctly.");
                    }             
                    if (request.ChangePasswordForm.NewPassword == request.ChangePasswordForm.OldPassword)
                    {
                        throw new Exception("New password cannot be the same as the old password. Please choose a different password.");
                    }
                }

                if (request.ChangePasswordForm.NewPassword != request.ChangePasswordForm.ConfirmPassword)
                {
                    throw new Exception("The new password and the confirmed password do not match. Please ensure that both passwords are entered correctly and match each other.");
                }

                account.Password = BCrypt.Net.BCrypt.HashPassword(request.ChangePasswordForm.NewPassword);
                account.CreatedAt = account.CreatedAt.ToUniversalTime(); 
                
                await Task.FromResult(_context.Accounts.Update(account));
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