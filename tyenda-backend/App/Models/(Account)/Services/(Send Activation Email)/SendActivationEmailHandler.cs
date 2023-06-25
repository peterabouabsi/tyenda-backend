using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Token_;
using tyenda_backend.App.Services.Email_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_
{
    public class SendActivationEmailHandler : IRequestHandler<SendActivationEmail, bool>
    {
        private readonly TyendaContext _context;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public SendActivationEmailHandler(TyendaContext context, IEmailService emailService, IConfiguration configuration, ITokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(SendActivationEmail request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(account => 
                    account.Email == request.SendActivationEmailForm.Email,
                    cancellationToken);

                if (account == null)
                {
                    throw new Exception("Account not found");
                }

                if (account.Active)
                {
                    throw new Exception("Account already activated");
                }
                
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("AccountId", account.Id.ToString())
                };
                var expiresIn = DateTime.UtcNow.AddMinutes(5);
                var token = _tokenService.GenerateRandomToken(claims, expiresIn);
                var newToken = new Token()
                {
                    Id = Guid.NewGuid(),
                    Value = token,
                    ExpiresIn = expiresIn,
                    AccountId = account.Id
                };

                await _context.Tokens.AddAsync(newToken, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                var activationLink = _configuration["Jwt:audience"] + "/authentication/email-verification?token="+token;
                var body = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; font-size: 14px;'>
                        <p><b>Activate your account now!</b></p>  
                        <p>Thank you for joining our platform! To activate your account, simply click on the link below:</p>
                        <a href='{activationLink}' style='color: #0072C6;'>Activate account</a>
                        <p>Once you've clicked on the link, your account will be activated and you can start enjoying all the features we have to offer.</p>
                        <p>If you have any questions or need assistance, please don't hesitate to reach out to our support team at [Support Email]. We're here to help!</p>
                        <p>Welcome aboard, and we look forward to serving you.</p>
                        <p>Best regards,<br/>Tyenda Team<br/></p>
                    </body>
                </html>";
                await _emailService.SendEmailAsync(account.Email, "Activate Account",  "html", body);
                return true;
            }
            
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}