using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Follower_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Follow_
{
    public class FollowHandler : IRequestHandler<Follow, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public FollowHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        public async Task<bool> Handle(Follow request, CancellationToken cancellationToken)
        {
            try
            {
                var storeId = Guid.Parse(request.FollowForm.StoreId);
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var follower = await _context.Followers.SingleOrDefaultAsync(follower => 
                        follower.StoreId == storeId && customer.Id == follower.CustomerId
                    , cancellationToken);

                var isFollowed = false;
                if (follower == null)
                {
                    //Follow
                    var newFollower = new Follower()
                    {
                        StoreId = storeId,
                        CustomerId = customer.Id
                    };
                    await _context.Followers.AddAsync(newFollower, cancellationToken);
                    isFollowed = true;
                }
                else
                {
                    //Unfollow
                    await Task.FromResult(_context.Followers.Remove(follower));
                    isFollowed = false;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return isFollowed;

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}