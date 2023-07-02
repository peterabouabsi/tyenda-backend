using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Top_Selling_Item_
{
    public class TopSellingItemHandler : IRequestHandler<TopSellingItem, Item>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public TopSellingItemHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Item> Handle(TopSellingItem request, CancellationToken cancellationToken)
        {
            try
            {

                var accountId = _tokenService.GetHeaderTokenClaim("AccountId");
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                var maxCountGroup = await _context.Orders
                    .Where(order => order.OrderStatus == OrderStatus.Completed)
                    .GroupBy(order => order.ItemId)
                    .Select(group => new
                    {
                        ItemId = group.Key,
                        Count = group.Count()
                    })
                    .OrderByDescending(group => group.Count)
                    .FirstOrDefaultAsync(cancellationToken);

                var itemId = maxCountGroup?.ItemId;

                var item = await _context.Items
                    .Include(item => item.Images)
                    .Include(item => item.Rates)
                    .Include(item => item.Carts.Where(cart => cart.CustomerId == customer!.Id))
                    .Include(item => item.Store)
                    .ThenInclude(store => store!.Account)
                    .SingleOrDefaultAsync(item => item.Id == itemId, cancellationToken);

                if (item == null)
                {
                    throw new Exception("No items sold");
                }
                return item;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}