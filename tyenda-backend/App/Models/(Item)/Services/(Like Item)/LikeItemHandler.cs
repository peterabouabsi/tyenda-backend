using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Like_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Like_Item_
{
    public class LikeItemHandler : IRequestHandler<LikeItem, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public LikeItemHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        public async Task<bool> Handle(LikeItem request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var itemId = Guid.Parse(request.LikeItemForm.ItemId);
                var customerId = customer.Id;

                var myItemLike = await _context.Likes.SingleOrDefaultAsync(like => like.ItemId == itemId && like.CustomerId == customerId, cancellationToken);
                var isItemLiked = false;
                
                if (myItemLike == null)
                {
                    //Like
                    var newLike = new Like()
                    {
                        ItemId = itemId,
                        CustomerId = customerId
                    };
                    await _context.Likes.AddAsync(newLike, cancellationToken);
                    isItemLiked = true;
                }
                else
                {
                    //Dislike
                    await Task.FromResult(_context.Likes.Remove(myItemLike));
                    isItemLiked = false;
                }

                await _context.SaveChangesAsync(cancellationToken);
                
                return isItemLiked;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}