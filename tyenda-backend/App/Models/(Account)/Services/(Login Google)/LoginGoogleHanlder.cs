using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Role_;
using tyenda_backend.App.Models._Session_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Login_Google_
{
    public class LoginGoogleHanlder : IRequestHandler<LoginGoogle, object>
    {
        private readonly TyendaContext _context;
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public LoginGoogleHanlder(TyendaContext context, IConfiguration configuration, IMediator mediator, ITokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _mediator = mediator;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(LoginGoogle request, CancellationToken cancellationToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings() {
                    Audience = new List<string?> { _configuration["GoogleOAuth:ClientId"] }
                 };

                var credentials = request.LoginGoogleForm.Credential;
                var payload = await GoogleJsonWebSignature.ValidateAsync(credentials, settings);
                if (payload != null)
                {
                    if (!payload.EmailVerified)
                    {
                        throw new Exception(payload.Email + " is not verified");
                    }
                    
                    //Check if user already exist
                    var account = await _context.Accounts
                        .Include(account => account.Role)
                        .SingleOrDefaultAsync(account => account.Email == payload.Email, cancellationToken);
                    
                    //If doesn't exist
                    Guid accountId;
                    Role customerRole;
                    if (account == null)
                    {
                        //Create a new Account - Customer    
                        var role = await _context.Roles.SingleOrDefaultAsync(role => role.Value == Constants.CustomerRole, cancellationToken);

                        var newAccount = new Account()
                        {
                            Id = Guid.NewGuid(),
                            Username = payload.Name.Replace(" ", "").ToLower(),
                            Email = payload.Email,
                            Password = null,
                            RoleId = role!.Id,
                            CreatedAt = DateTime.UtcNow,
                            Active = true,
                            PhoneNumber = null,
                            ProfileImage = null
                        };
                        
                        string fullName = payload.Name;
                        int lastIndex = fullName.LastIndexOf(payload.FamilyName, StringComparison.Ordinal)!;
                        string firstName = fullName.Remove(lastIndex, payload.FamilyName.Length).Trim();
                        
                        var newCustomer = new Customer()
                        {
                            Id = Guid.NewGuid(),
                            Firstname = firstName,
                            Lastname = payload.FamilyName,
                            OnItem = false,
                            AccountId = newAccount.Id
                        };

                        await _context.Accounts.AddAsync(newAccount, cancellationToken);
                        await _context.Customers.AddAsync(newCustomer, cancellationToken);
                        
                        accountId = newAccount.Id;
                        customerRole = role;
                    }
                    else
                    {
                        
                        accountId = account.Id;
                        customerRole = account.Role!;
                    }
                    
                    //Login - Generate Token
                    var tokenClaims = new List<Claim>()
                    {
                        new Claim("AccountId", accountId.ToString()),
                        new Claim("Role", customerRole!.Value)
                    };

                    var accessToken = _tokenService.GenerateAccessToken(tokenClaims);
                    var refreshToken = _tokenService.GenerateRefreshToken(tokenClaims);

                    var newSession = new Session()
                    {
                        Id = Guid.NewGuid(),
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresIn = _tokenService.GetTokenExpiryDate(refreshToken),
                        AccountId = accountId
                    };
                    
                    await _context.Sessions.AddAsync(newSession, cancellationToken);

                    var loginResponse = new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccountId = accountId
                    };
                    
                    await _context.SaveChangesAsync(cancellationToken);

                    return loginResponse;
                }
                else
                {
                    throw new Exception("Something went wrong, payload is null");
                }
            }
            catch (Exception ex)
            {
                // Log the error and print the inner exception details
                Console.WriteLine("Error occurred while saving changes: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                throw;
            }
        }
    }
}