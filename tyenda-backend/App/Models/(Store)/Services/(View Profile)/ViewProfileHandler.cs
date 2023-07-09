using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Models._Store_.Views;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._View_Profile_
{
    public class ViewProfileHandler : IRequestHandler<ViewProfile, StoreAdvancedView>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public ViewProfileHandler(TyendaContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<StoreAdvancedView> Handle(ViewProfile request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var customerId = customer.Id;
                var storeId = Guid.Parse(request.StoreId);
                
                var store = await _context.Stores
                    .Include(store => store.Account)
                    .Include(store => store.Branches)
                        .ThenInclude(branch => branch.City)
                        .ThenInclude(city => city!.Country)
                    .Include(store => store.Categories)
                        .ThenInclude(storeCategory => storeCategory.Category)
                    .Include(store => store.Followers)
                    .Include(store => store.Carts)
                    .Include(store => store.Items)
                        .ThenInclude(item => item.Orders)

                    .SingleOrDefaultAsync(store => store.Id == storeId, cancellationToken);

                if (store == null)
                {
                    throw new Exception("Store not found");
                }

                var mappedStore = _mapper.Map<StoreAdvancedView>(store);

                mappedStore.IsFollowed = store.Followers.Any(follower => follower.CustomerId == customerId);
                mappedStore.IsAddedToCart = store.Carts.Any(cart => cart.CustomerId == customerId);

                return mappedStore;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}