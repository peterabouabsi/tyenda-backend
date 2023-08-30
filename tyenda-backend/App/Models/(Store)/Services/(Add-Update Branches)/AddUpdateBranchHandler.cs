using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Branches_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Add_Update_Branches_
{
    public class AddUpdateBranchHandler : IRequestHandler<AddUpdateBranch, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public AddUpdateBranchHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(AddUpdateBranch request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts.Include(account => account.Store).SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);

                if (account == null) throw new Exception("Account not found");
                if (account.Store == null) throw new Exception("Store not found");

                var existingBranches = await _context.Branches.Where(branch => branch.StoreId == account.Store.Id).ToArrayAsync(cancellationToken);
                _context.Branches.RemoveRange(existingBranches);
                foreach (var form in request.AddUpdateBranchForm)
                {
                    var newBranch = new Branch()
                    {
                        Id = Guid.NewGuid(),
                        AddressDetails = form.AddressDetails,
                        Latitude = form.Lat,
                        Longitude = form.Lng,
                        CityId = Guid.Parse(form.CityId),
                        StoreId = account.Store.Id
                    };
                    await _context.Branches.AddAsync(newBranch, cancellationToken);
                }

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