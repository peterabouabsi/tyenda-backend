using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Delete_Item_
{
    public class DeleteItemHandler : IRequestHandler<DeleteItem, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public DeleteItemHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(DeleteItem request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);

                var store = await _context.Stores.SingleOrDefaultAsync(store => 
                    store.AccountId == Guid.Parse(accountId) , cancellationToken);
                if (store == null) throw new Exception("Store not found");

                var item = await _context.Items.SingleOrDefaultAsync(item => 
                    item.Id == Guid.Parse(request.ItemId) && item.StoreId == store.Id, cancellationToken);

                if (item == null) throw new Exception("Something went wrong! Item not found. Please check if you have the permission to delete this item.");

                await Task.FromResult(_context.Items.Remove(item));
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}