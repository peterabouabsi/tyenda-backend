using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Order_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._My_Item_Orders_
{
    public class MyItemOrdersHandler : IRequestHandler<MyItemOrders, ICollection<Order>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public MyItemOrdersHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Order>> Handle(MyItemOrders request, CancellationToken cancellationToken)
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
                var itemId = Guid.Parse(request.ItemId);

                var orders = await _context.Orders
                    .Where(order => order.ItemId == itemId)
                    .Include(order => order.Item)
                    .ThenInclude(item => item!.Orders.Where(order => order.CustomerId == customerId))
                    .Include(order => order.Item!.Store)
                    .ThenInclude(item => item!.Account)
                    .Include(order => order.Customer)
                    .Include(order => order.City)
                    .ThenInclude(city => city!.Country)
                    .ToArrayAsync(cancellationToken);

                return orders;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}