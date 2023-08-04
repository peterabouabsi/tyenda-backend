using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Get_Recent_Orders_
{
    public class GetRecentOrdersHandler : IRequestHandler<GetRecentOrders, ICollection<Order>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetRecentOrdersHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Order>> Handle(GetRecentOrders request, CancellationToken cancellationToken)
        {
            try
            {
                
                var accountId = Guid.Parse(_tokenService.GetHeaderTokenClaim(Constants.AccountId));
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .SingleOrDefaultAsync(account => account.Id == accountId, cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }

                if (account.Role!.Value == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == accountId,cancellationToken);
                
                    if (customer == null)
                    {
                        throw new UnauthorizedAccessException("Customer not found");
                    }
                    
                    var orders = await _context.Orders
                        .Include(order => order.OrderItems)
                        .Include(order => order.Customer)
                        .Include(order => order.Item)
                        .ThenInclude(item => item!.Store)
                        .ThenInclude(item => item!.Account)
                        .Include(order => order.City)
                        .ThenInclude(city => city!.Country)
                        .Where(order => order.CustomerId == customer.Id)
                        .OrderByDescending(order => order.CreatedAt)
                        .Take(4)
                        .ToArrayAsync(cancellationToken);

                    return orders;                    
                }

                else if (account.Role!.Value == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == accountId,
                        cancellationToken);

                    if (store == null)
                    {
                        throw new UnauthorizedAccessException("Store not found");
                    }

                    var orders = await _context.Orders
                        .Include(order => order.OrderItems)
                        .Include(order => order.Customer)
                        .Include(order => order.Item)
                        .ThenInclude(item => item!.Store)
                        .ThenInclude(item => item!.Account)
                        .Include(order => order.City)
                        .ThenInclude(city => city!.Country)
                        .Where(order => order.Item!.StoreId == store.Id)
                        .OrderByDescending(order => order.CreatedAt)
                        .Take(4)
                        .ToArrayAsync(cancellationToken);

                    return orders;
                }

                else throw new UnauthorizedAccessException("Something went wrong. you don't have the permission to access data!");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}