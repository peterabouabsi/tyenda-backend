using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Get_Similar_Stores_
{
    public class GetSimilarStoresHandler : IRequestHandler<GetSimilarStores, ICollection<Store>>
    {
        
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetSimilarStoresHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Store>> Handle(GetSimilarStores request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);

                var myStore = await _context.Stores
                    .Include(store => store.Categories)
                    .SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);

                if (myStore == null) throw new UnauthorizedAccessException("Store not found");
                
                var storeCategories = myStore.Categories.Select(category => category.CategoryId);
                
                var otherStores = await _context.Stores
                    .Where(store => store.AccountId != Guid.Parse(accountId))
                    .Include(store => store.Account)
                    .Include(store => store.Categories.Where(category => storeCategories.Contains(category.CategoryId)))
                    .ToArrayAsync(cancellationToken);

                int? take = request.Take ?? -1;
                List<Store> similarStores = new List<Store>();
                foreach (var store in otherStores)
                {
                    if (store.Categories.Count >= 3)
                    {
                        if (take != -1)
                        {
                            if (take > 0)
                            {
                                similarStores.Add(store);
                                take--;
                            }
                            else
                            {
                                break;
                            }                            
                        }

                    }
                }
                
                return similarStores;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}