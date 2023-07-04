using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Comment_.Views;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Comment_.Services._Get_Item_Comments_
{
    public class GetItemCommentsHandler : IRequestHandler<GetItemComments, ICollection<CommentAdvancedView>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public GetItemCommentsHandler(TyendaContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<ICollection<CommentAdvancedView>> Handle(GetItemComments request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = Guid.Parse(_tokenService.GetHeaderTokenClaim(Constants.AccountId));
                var role = _tokenService.GetHeaderTokenClaim(Constants.Role);
                var itemId = Guid.Parse(request.ItemId);
                
                if (role == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == accountId, cancellationToken);
                    var customerId = customer!.Id;
                    
                    var comments = await _context.Comments
                        .Where(comment => comment.ItemId == itemId)
                        .Include(comment => comment.Customer)
                        .ThenInclude(prop => prop!.Account)
                        .OrderByDescending(comment => comment.CreatedAt)
                        .ToArrayAsync(cancellationToken);

                    var mappedComments = _mapper.Map<ICollection<CommentAdvancedView>>(comments);
                    foreach (var comment in mappedComments)
                    {
                        if (comment.CustomerId != customerId.ToString())
                        {
                            comment.ShowDeleteOption = false;
                        }
                    }
                    
                    return mappedComments;
                }
                
                if (role == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == accountId, cancellationToken);
                    var storeId = store!.Id;

                    var isItemForStoreId = await Task.FromResult(_context.Items.Any(item => item.StoreId == storeId));
                    if (!isItemForStoreId)
                    {
                        throw new Exception("Item doesn't belong to the store "+store.Name);
                    }
                    
                    var comments = await _context.Comments
                        .Where(comment => comment.ItemId == itemId)
                        .Include(comment => comment.Customer)
                        .ThenInclude(customer => customer!.Account)
                        .OrderBy(comment => comment.CreatedAt)
                        .ToArrayAsync(cancellationToken);

                    var mappedComments = _mapper.Map<ICollection<CommentAdvancedView>>(comments);
                    return mappedComments;
                }

                return null!;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}