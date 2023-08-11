using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._OrderItem_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Delete_Order_
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrder, Order>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public DeleteOrderHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Order> Handle(DeleteOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts.Include(account => account.Role).SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null) throw new Exception("Account not found");
                
                var accountRole = account.Role!.Value;

                var order = await _context.Orders
                    .Include(order => order.Item)
                    .ThenInclude(item => item!.Images)
                    .Include(order => order.Item)
                    .ThenInclude(item => item!.Store)
                    .ThenInclude(item => item!.Account)
                    .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.Color)
                    .Include(order => order.City)
                    .ThenInclude(city => city!.Country)
                    .Include(order => order.Customer)
                    .ThenInclude(city => city!.Account)
                    .Include(order => order.Feedbacks.OrderByDescending(feedback => feedback.CreatedAt))
                    .ThenInclude(feedback => feedback.Customer)
                    .ThenInclude(orderCustomer => orderCustomer!.Account)
                    .SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.OrderId), cancellationToken);
                
                if (order == null) throw new Exception("Order not found");

                if (accountRole == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                    if (customer == null) throw new Exception("Customer not found");
                    
                    if (order.CustomerId == customer.Id)
                    {
                        await Task.FromResult(_context.Orders.Remove(order));   
                    }
                    else
                    {
                        throw new Exception("You don't own this order");
                    }
                }

                if (accountRole == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);

                    if (store == null) throw new Exception("Store not found");
                    
                    if (order.Item!.StoreId == store.Id)
                    {
                        await Task.FromResult(_context.Orders.Remove(order));
                    }
                    else
                    {
                        throw new Exception("You don't own this order");
                    }
                }
                
                await _context.SaveChangesAsync(cancellationToken);
                return order;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}