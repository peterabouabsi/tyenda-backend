using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._AddToCart_
{
    public class AddToCartHandler : IRequestHandler<AddToCart, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public AddToCartHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(AddToCart request, CancellationToken cancellationToken)
        {
            try
            {
                var storeId = Guid.Parse(request.AddToCartForm.StoreId!);
                var accountId = _tokenService.GetHeaderTokenClaim("AccountId");
                var customer = await _context.Customers.SingleOrDefaultAsync(customer =>
                        customer.AccountId == Guid.Parse(accountId)
                    ,cancellationToken);

                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }
                
                var myCart = await _context.Carts.SingleOrDefaultAsync(cart => 
                    cart.StoreId == storeId && cart.CustomerId == customer.Id, 
                    cancellationToken);

                var isAddedToCart = false;
                if (myCart == null)
                {
                    //Add to cart
                    var newCart = new Cart()
                    {
                        Id = Guid.NewGuid(),
                        StoreId = storeId,
                        CustomerId = customer.Id
                    };
                    await _context.Carts.AddAsync(newCart, cancellationToken);
                    isAddedToCart = true;
                }
                else
                {
                    //Remove from cart
                    await Task.FromResult(_context.Carts.Remove(myCart));
                    isAddedToCart = false;
                }

                await _context.SaveChangesAsync(cancellationToken);

                return isAddedToCart;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}