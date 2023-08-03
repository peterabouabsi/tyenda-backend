using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Get_Profile_
{
    public class GetProfileHandler : IRequestHandler<GetProfile, object>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetProfileHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(GetProfile request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .Include(account => account.Customer)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }

                var profileData = new object();
                if (account.Role!.Value == Constants.CustomerRole)
                {
                    profileData = new
                    {
                        AccountId = account.Id,
                        CustomerId = account.Customer!.Id,
                        Firstname = account.Customer.Firstname,
                        Lastname = account.Customer.Lastname,
                        Email = account.Email,
                        Phone = account.PhoneNumber,
                        ProfileImage = account.ProfileImage
                    };
                }
                
                if (account.Role!.Value == Constants.StoreRole)
                {
                    
                }

                return profileData;


            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}