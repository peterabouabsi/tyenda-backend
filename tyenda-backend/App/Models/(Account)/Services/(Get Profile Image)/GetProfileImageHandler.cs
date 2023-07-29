using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Get_Profile_Image_
{
    public class GetProfileImageHandler : IRequestHandler<GetProfileImage, string>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetProfileImageHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(GetProfileImage request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts.SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId),cancellationToken);
                return account!.ProfileImage!;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}