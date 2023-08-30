using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Get_Branches_
{
    public class GetBranchesHandler : IRequestHandler<GetBranches, ICollection<object>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetBranchesHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<object>> Handle(GetBranches request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts.Include(account => account.Store).SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);

                if (account == null) throw new Exception("Account not found");
                if (account.Store == null) throw new Exception("Store not found");

                var branches = await _context.Branches
                    .Where(branch => branch.StoreId == account.Store.Id)
                    .Include(branch => branch.City)
                    .ThenInclude(branch => branch!.Country)
                    .Select(branch => new
                    {
                        Id = branch.Id,
                        City = new {Id = branch.CityId, Value = branch.City!.Value},
                        Country = new {Id = branch.City!.CountryId, Value = branch.City.Country!.Value},
                        AddressDetails = branch.AddressDetails,
                        Lat = branch.Latitude,
                        Lng = branch.Longitude
                    })
                    .ToArrayAsync(cancellationToken);

                return branches;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}