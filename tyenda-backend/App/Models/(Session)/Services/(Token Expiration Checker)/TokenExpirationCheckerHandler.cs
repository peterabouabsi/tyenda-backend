﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Session_.Views;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Session_.Services._Token_Expiration_Checker_
{
    public class TokenExpirationCheckerHandler : IRequestHandler<TokenExpirationChecker, TokenExpirationCheckerView>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        
        public TokenExpirationCheckerHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<TokenExpirationCheckerView> Handle(TokenExpirationChecker request, CancellationToken cancellationToken)
        {
            try
            {

                var token = request.TokenExpirationCheckerForm.Token;
                if (token == "")
                {
                    throw new Exception("Required token");
                }
                else
                {
                    var tokenAccountId = Guid.Parse(_tokenService.GetTokenClaim(token, Constants.AccountId));
                    var session = await _context.Sessions.SingleOrDefaultAsync(session => session.AccessToken == token && session.AccountId == tokenAccountId, cancellationToken);
                    
                    var tokenExpirationCheckerView = new TokenExpirationCheckerView()
                    {
                        IsExpired = false
                    };
                    
                    if (session != null)
                    {
                        var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        var tokenExpirationDate = _tokenService.GetTokenExpiryDate(token);
                    
                        if (tokenExpirationDate <= now)
                        {
                            tokenExpirationCheckerView.IsExpired = true;
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid token");
                    }
                    
                    return tokenExpirationCheckerView;
                }

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}