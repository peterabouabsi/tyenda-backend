using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Cart_.Services._Item_Cart_Update_
{
    public class ItemCartUpdateHandler : IRequestHandler<ItemCartUpdate, int>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public ItemCartUpdateHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<int> Handle(ItemCartUpdate request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var customerId = customer.Id;
                var itemId = Guid.Parse(request.ItemCartUpdateForm.ItemId);
                var quantity = request.ItemCartUpdateForm.Quantity;

                var cart = await _context.Carts.SingleOrDefaultAsync(cart => cart.ItemId == itemId && cart.CustomerId == customerId, cancellationToken);
                if (cart == null)
                {
                    throw new Exception("Cart not found");
                }

                cart.Quantity = quantity;
                await Task.FromResult(_context.Carts.Update(cart));
                await _context.SaveChangesAsync(cancellationToken);
                
                return (int) cart.Quantity;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}