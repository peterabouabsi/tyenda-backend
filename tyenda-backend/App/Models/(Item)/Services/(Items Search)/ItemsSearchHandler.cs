using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Items_Search_
{
    public class ItemsSearchHandler : IRequestHandler<ItemsSearch, ICollection<Item>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        
        public ItemsSearchHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Item>> Handle(ItemsSearch request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null) throw new UnauthorizedAccessException("Account not found");

                IQueryable<Item> query = _context.Items;
                if (account.Role!.Value == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                    if (customer == null)
                    {
                        throw new UnauthorizedAccessException("Customer not found");
                    }
                    
                    query = query
                        .Include(item => item.Images.OrderBy(image => image.CreatedAt))
                        .Include(item => item.Rates)
                        .Include(item => item.Carts.Where(cart => cart.CustomerId == customer.Id))
                        .Include(item => item.Store)
                        .ThenInclude(store => store!.Account)
                        .Include(item => item.Likes.Where(like => like.CustomerId == customer.Id));
                }
                if (account.Role!.Value == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(
                        store => store.AccountId == Guid.Parse(accountId), cancellationToken);
                   
                    if (store == null)
                    {
                        throw new UnauthorizedAccessException("Store not found");
                    }

                    query = query
                        .Where(item => item.StoreId == store.Id)
                        .Include(item => item.Images.OrderBy(image => image.CreatedAt))
                        .Include(item => item.Rates)
                        .Include(item => item.Store)
                        .ThenInclude(prop => prop!.Account);
                }

                var name = request.SearchForm.Name;
                var createdAt = request.SearchForm.CreatedAt;
                var price = request.SearchForm.Price;
                var categories = request.SearchForm.Categories;
                
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(item => item.Value.ToLower().Contains(name.ToLower()));
                }

                if (createdAt != null)
                {
                    query = query.Where(item => item.CreatedAt >= createdAt.Value.ToUniversalTime());
                }
                
                if (categories is {Length: > 0})
                {
                    foreach (var category in categories)
                    {
                        query = query.Where(item => item.Categories.Any(categoryObj => categoryObj.CategoryId == Guid.Parse(category)));
                    }
                }
                
                var items = await query
                    .Where(item => item.Price >= price![0] && item.Price <= price![1])
                    .OrderByDescending(item => item.CreatedAt)
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