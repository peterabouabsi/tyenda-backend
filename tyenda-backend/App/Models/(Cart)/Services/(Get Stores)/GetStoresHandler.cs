using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Cart_.Services._Get_Stores_
{
    public class GetStoresHandler : IRequestHandler<GetStores, ICollection<Cart>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetStoresHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Cart>> Handle(GetStores request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim("AccountId");
                var customer = await _context.Customers
                    .SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var customerId = customer.Id;

                var storesCart = await _context.Carts
                    .Where(cart => cart.CustomerId == customerId && cart.StoreId != null)
                    .Include(cart => cart.Store)
                    .ThenInclude(store => store!.Account)
                    .Include(cart => cart.Store!.Items)
                    .ThenInclude(item => item.Orders)
                    .Include(cart => cart.Store!.Followers)
                    .OrderByDescending(cart => cart.Store!.Account!.CreatedAt)
                    .Skip(request.Skip)
                    .Take(request.Top)
                    .ToArrayAsync(cancellationToken);

                return storesCart;
                
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}