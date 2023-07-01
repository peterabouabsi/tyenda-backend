using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Add_Remove_Cart_
{
    public class AddRemoveCartHandler : IRequestHandler<AddRemoveCart, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;


        public AddRemoveCartHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(AddRemoveCart request, CancellationToken cancellationToken)
        {
            try
            {

                var accountId = _tokenService.GetHeaderTokenClaim("AccountId");
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var customerId = customer.Id;
                var itemId = Guid.Parse(request.AddRemoveCartForm.ItemId!);

                var myCart = await _context.Carts.SingleOrDefaultAsync(cart => cart.ItemId == itemId && cart.CustomerId == customerId,cancellationToken);

                var isAddedToCart = false;
                if (myCart == null)
                {
                    //Add to cart
                    var newCart = new Cart()
                    {
                        Id = Guid.NewGuid(),
                        ItemId = itemId,
                        CustomerId = customerId,
                        Quantity = 1
                    };
                    await _context.Carts.AddAsync(newCart, cancellationToken);
                    isAddedToCart = true;
                }
                else
                {
                    //Update from cart
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