using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Store_.Views;
using tyenda_backend.App.Models.Configs;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Get_Random_Stores_
{
    public class GetRandomStoresHandler : IRequestHandler<GetRandomStores, PagerDataConfig<StoreModerateView>>
    {
        private readonly TyendaContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GetRandomStoresHandler(TyendaContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<PagerDataConfig<StoreModerateView>> Handle(GetRandomStores request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }
                
                var top = request.Top;
                var skip = request.Skip;
                
                var data = await _context.Stores
                    .Include(store => store.Account)
                    .Include(store => store.Followers.Where(follower => follower.CustomerId == customer.Id))
                    .Include(store => store.Carts.Where(cart => cart.CustomerId == customer.Id))
                    .OrderBy(store => store.Id)
                    .Skip(skip)
                    .Take(top)
                    .ToArrayAsync(cancellationToken);
                var count = await _context.Stores.CountAsync(cancellationToken);

                var stores = _mapper.Map<ICollection<StoreModerateView>>(data);
                return new PagerDataConfig<StoreModerateView>()
                {
                    Data = stores,
                    Count = count
                };
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}