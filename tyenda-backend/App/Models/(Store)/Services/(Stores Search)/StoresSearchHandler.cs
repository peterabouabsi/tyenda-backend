using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Stores_Search_
{
    public class StoresSearchHandler : IRequestHandler<StoresSearch, ICollection<Store>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public StoresSearchHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Store>> Handle(StoresSearch request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                IQueryable<Store> query = _context.Stores
                    .Include(store => store.Account)
                    .Include(store => store.Followers.Where(follower => follower.CustomerId == customer.Id))
                    .Include(store => store.Carts.Where(cart => cart.CustomerId == customer.Id))
                    .OrderBy(store => store.Id);

                var name = request.SearchForm.Name;
                var createdAt = request.SearchForm.CreatedAt;
                var city = request.SearchForm.City;
                var categories = request.SearchForm.Categories;

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(store => store.Name.ToLower().Contains(name.ToLower()));
                }
                
                if (!string.IsNullOrEmpty(city))
                {
                    query = query.Where(store => store.Branches.Any(branch => branch.CityId == Guid.Parse(city)));
                }

                if (createdAt != null)
                {
                    query = query.Where(store => store.Account!.CreatedAt >= createdAt.Value.ToUniversalTime());
                }
                
                if (categories is {Length: > 0})
                {
                    foreach (var category in categories)
                    {
                        query = query.Where(store => store.Categories.Any(categoryObj => categoryObj.CategoryId == Guid.Parse(category)));
                    }
                }
                
                var stores = await query
                    .OrderByDescending(store => store.Account!.CreatedAt)
                    .ToListAsync(cancellationToken);

                return stores;

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}