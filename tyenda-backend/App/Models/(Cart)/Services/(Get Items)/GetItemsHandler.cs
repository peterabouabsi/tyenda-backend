using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Cart_.Services._Get_Items_
{
    public class GetItemsHandler : IRequestHandler<GetItems, ICollection<Cart>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetItemsHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Cart>> Handle(GetItems request, CancellationToken cancellationToken)
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
                var itemsCart = await _context.Carts
                    .Where(cart => cart.CustomerId == customerId && cart.ItemId != null)
                    .Include(cart => cart.Item!)
                    .ThenInclude(item => item.Likes)
                    .Include(cart => cart.Item!.Rates)
                    .Include(cart => cart.Item!.Orders)
                    .Include(cart => cart.Item!.Images)
                    .Include(cart => cart.Item!.Store)
                    .ThenInclude(store => store!.Account)
                    .OrderByDescending(cart => cart.Store!.Account!.CreatedAt)
                    .Take(request.Top)
                    .ToArrayAsync(cancellationToken);

                return itemsCart;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}