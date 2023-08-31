using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Orders_Search_
{
    public class OrdersSearchHandler : IRequestHandler<OrdersSearch, ICollection<Order>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public OrdersSearchHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Order>> Handle(OrdersSearch request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);

                if (account == null) throw new UnauthorizedAccessException("Account not found");
                
                IQueryable<Order> query = _context.Orders
                    .Include(order => order.OrderItems)
                    .Include(order => order.Item)
                    .ThenInclude(item => item!.Store)
                    .ThenInclude(item => item!.Account)
                    .Include(order => order.Customer)
                    .Include(order => order.City)
                    .ThenInclude(city => city!.Country);

                if (account.Role!.Value == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                    if (customer == null)
                    {
                        throw new UnauthorizedAccessException("Customer not found");
                    }

                    var customerId = customer.Id;
                    query =  query.Where(order => order.CustomerId == customerId);
                }

                if (account.Role!.Value == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(store =>
                        store.AccountId == Guid.Parse(accountId), cancellationToken);
                    
                    if (store == null)
                    { 
                        throw new UnauthorizedAccessException("Store not found");
                    }
                    var storeId = store.Id;
                    query =  query.Where(order => order.Item!.StoreId == storeId);
                }
                
                var keyword = request.SearchForm.Keyword;
                var reference = request.SearchForm.Reference;
                var minDate = request.SearchForm.MinDate;
                var orderStatuses = request.SearchForm.OrderStatuses;

                // Apply keyword filter if provided
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(order =>
                        order.Item!.Value.ToLower().Contains(keyword.ToLower()) || order.Note.ToLower().Contains(keyword.ToLower()) ||
                        order.Customer!.Firstname.ToLower().Contains(keyword.ToLower()) || order.Customer!.Lastname.ToLower().Contains(keyword.ToLower()) ||
                        order.Item!.Store!.Name.ToLower().Contains(keyword.ToLower()) || order.City!.Value.ToLower().Contains(keyword.ToLower()) ||
                        order.ReceiverName!.ToLower().Contains(keyword.ToLower()) || order.City!.Country!.Value.ToLower().Contains(keyword.ToLower()));
                }

                // Apply reference filter if provided
                if (!string.IsNullOrEmpty(reference))
                {
                    query = query.Where(order => order.Reference.Contains(reference));
                }

                // Apply createdAt filter if provided
                if (minDate != null)
                {
                    query = query.Where(order => order.CreatedAt >= minDate.Value.ToUniversalTime());
                }

                // Apply orderStatuses filter if provided
                if (orderStatuses.Any())
                {
                    query = query.Where(order => orderStatuses.Contains(order.OrderStatus));
                }
                
                var orders = await query
                    .OrderByDescending(order => order.CreatedAt)
                    .ToListAsync(cancellationToken);
                
                return orders;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}
