using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Token_;
using tyenda_backend.App.Services.Email_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Forget_Password_
{
    public class ForgetPasswordHandler : IRequestHandler<ForgetPassword, object>
    {
        private readonly TyendaContext _context;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public ForgetPasswordHandler(TyendaContext context, IEmailService emailService, ITokenService tokenService, IConfiguration configuration, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _emailService = emailService;
            _tokenService = tokenService;
            _configuration = configuration;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<object> Handle(ForgetPassword request, CancellationToken cancellationToken)
        {
            try
            {
                var email = request.ForgetPasswordForm.Email;
                var account = await _context.Accounts
                    .Include(account => account.Customer)
                    .SingleOrDefaultAsync(
                    account => account.Email == email,
                    cancellationToken);

                if (account == null)
                {
                    throw new Exception("Account with email (" + email + ") not found");
                }

                //Send email
                var tokenClaims = new List<Claim>()
                {
                    new Claim("AccountId", account.Id.ToString())
                };

                var expiresIn = DateTime.UtcNow.AddMinutes(10);
                var newToken = new Token()
                {
                    Id = Guid.NewGuid(),
                    Value = _tokenService.GenerateRandomToken(tokenClaims, expiresIn),
                    ExpiresIn = expiresIn,
                    AccountId = account.Id
                };
                await _context.Tokens.AddAsync(newToken, cancellationToken);
                
                // Create the HTML body of the message
                var resetLink = _configuration["Urls:Frontend"]+"/authentication/reset-password?token="+newToken.Value;
                var subject = "Reset Password";
                var body = $@"<html>
                <body style='font-family: Arial, sans-serif; font-size: 14px;'>
                <p>Dear <b>{account.Customer!.Firstname}</b>,</p>
                <p>We have received a password reset request for your account. To ensure the security of your account, please follow the instructions below to reset your password:</p>
                <ol>
                <li>Click on the following link to access the password reset page: <a href='{resetLink}' style='color: #0072C6;'>Reset your password</a></li>
                <li>Once on the password reset page, you will be prompted to enter a new password. Please choose a strong and unique password that you have never used before.</li>
                <li>After entering your new password, click the Reset button.</li>
                </ol>
                <p>If you did not request a password reset, please disregard this email. Your account is still secure and no changes have been made.</p>
                <p>Best regards,<br/>Tyenda Team<br/></p>
                </body>
                </html>";
                _backgroundJobClient.Enqueue(() => _emailService.SendEmailAsync(account.Email, subject, "html", body));
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