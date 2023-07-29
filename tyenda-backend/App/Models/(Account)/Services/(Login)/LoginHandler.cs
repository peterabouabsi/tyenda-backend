using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Session_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Login_
{
    public class LoginHandler : IRequestHandler<Login, object>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public LoginHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(Login request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .SingleOrDefaultAsync(account =>
                    account.Username.ToLower() == request.LoginForm.UsernameOrEmail.Trim().ToLower() ||
                    account.Email.ToLower() == request.LoginForm.UsernameOrEmail.Trim().ToLower(), 
                    cancellationToken);

                if (account == null)
                {
                    throw new Exception("Username or email not found");
                }

                var pwdMatch = BCrypt.Net.BCrypt.Verify(request.LoginForm.Password,account.Password);
                if (!pwdMatch)
                {
                    throw new Exception("Password is incorrect");
                }
                
                if (account.Active)
                {
                    //Generate Token
                    var tokenClaims = new List<Claim>()
                    {
                        new Claim("AccountId", account.Id.ToString()),
                        new Claim("Role", account.Role!.Value)
                    };

                    var accessToken = _tokenService.GenerateAccessToken(tokenClaims);
                    var refreshToken = _tokenService.GenerateRefreshToken(tokenClaims);

                    var newSession = new Session()
                    {
                        Id = Guid.NewGuid(),
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresIn = _tokenService.GetTokenExpiryDate(refreshToken),
                        AccountId = account.Id
                    };
                    await _context.Sessions.AddAsync(newSession, cancellationToken);

                    var loginResponse = new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    };
                
                    account.CreatedAt = DateTime.SpecifyKind(account.CreatedAt, DateTimeKind.Utc);
                    _context.Accounts.Update(account);
                
                    await _context.SaveChangesAsync(cancellationToken);

                    return loginResponse;    
                }
                else
                {
                    var loginResponse = new
                    {
                        IsActive = false,
                        Email = account.Email
                    };
                    return loginResponse;
                }
                
                
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}