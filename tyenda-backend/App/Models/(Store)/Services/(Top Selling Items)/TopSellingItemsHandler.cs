using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._Store_.Services._Top_Selling_Items_
{
    public class TopSellingItemsHandler : IRequestHandler<TopSellingItems, ICollection<Item>>
    {
        private readonly TyendaContext _context;

        public TopSellingItemsHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Item>> Handle(TopSellingItems request, CancellationToken cancellationToken)
        {
            try
            {
                var storeId = Guid.Parse(request.StoreId);
                var store = await _context.Stores.SingleOrDefaultAsync(store => store.Id == storeId, cancellationToken);
                if (store == null)
                {
                    throw new Exception("Store not found");
                }

                var items = await _context.Items
                    .Where(item => item.StoreId == storeId && item.Orders.Any(order => order.OrderStatus == OrderStatus.Completed))
                    .Include(item => item.Orders)
                    .Include(item => item.Likes)
                    .Include(item => item.Rates)
                    .Include(item => item.Images)
                    .OrderByDescending(item => item.Orders.Count)
                    .ToArrayAsync(cancellationToken);

                return items;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}