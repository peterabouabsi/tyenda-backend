﻿using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_
{
    public class GetItemHandler : IRequestHandler<GetItem, ItemAdvancedView>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public GetItemHandler(TyendaContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<ItemAdvancedView> Handle(GetItem request, CancellationToken cancellationToken)
        {
            try
            {
                
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);

                if (account == null) throw new UnauthorizedAccessException("Account not found");

                var itemId = Guid.Parse(request.ItemId);
                var mappedItem = new ItemAdvancedView(){};
                
                if (account.Role!.Value == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                
                    if (customer == null)
                    {
                        throw new UnauthorizedAccessException("Customer not found");
                    }
                    var customerId = customer.Id;
                    var item = await _context.Items
                        .Include(item => item.Store).ThenInclude(store => store!.Account)
                        .Include(item => item.Categories).ThenInclude(category => category.Category)
                        .Include(item => item.Colors).ThenInclude(color => color.Color)
                        .Include(item => item.Sizes)
                        .Include(item => item.Rates)
                        .Include(item => item.Carts.Where(cart => cart.CustomerId == customerId))
                        .Include(item => item.Images)
                        .Include(item => item.Notes)
                        .Include(item => item.Likes)
                        .Include(item => item.Orders)
                        .Include(item => item.Comments)
                        .ThenInclude(comment => comment.Customer)
                        .SingleOrDefaultAsync(item => item.Id == itemId, cancellationToken);
                
                    if (item == null)
                    {
                        throw new Exception("Item not found");
                    }

                    mappedItem = _mapper.Map<ItemAdvancedView>(item);
                    mappedItem.IsLiked = item.Likes.Any(like => like.CustomerId == customerId);
                    mappedItem.MyRate = item.Rates.SingleOrDefault(rate => rate.CustomerId == customerId)?.Rate ?? 0;

                }

                if (account.Role!.Value == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);
                
                    if (store == null)
                    {
                        throw new UnauthorizedAccessException("Store not found");
                    }
                    
                    var item = await _context.Items
                        .Include(item => item.Store).ThenInclude(store => store!.Account)
                        .Include(item => item.Categories).ThenInclude(category => category.Category)
                        .Include(item => item.Colors).ThenInclude(color => color.Color)
                        .Include(item => item.Sizes)
                        .Include(item => item.Rates)
                        .Include(item => item.Images)
                        .Include(item => item.Notes)
                        .Include(item => item.Likes)
                        .Include(item => item.Orders)
                        .Include(item => item.Comments)
                        .ThenInclude(comment => comment.Customer)
                        .SingleOrDefaultAsync(item => item.Id == itemId && item.StoreId == store.Id, cancellationToken);
                
                    if (item == null)
                    {
                        throw new Exception("Item not found");
                    }

                    mappedItem = _mapper.Map<ItemAdvancedView>(item);
                }
                
                return mappedItem;                
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}