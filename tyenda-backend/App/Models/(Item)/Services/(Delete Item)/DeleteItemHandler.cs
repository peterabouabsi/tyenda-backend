using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.File_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Delete_Item_
{
    public class DeleteItemHandler : IRequestHandler<DeleteItem, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;

        public DeleteItemHandler(TyendaContext context, ITokenService tokenService, IFileService fileService)
        {
            _context = context;
            _tokenService = tokenService;
            _fileService = fileService;
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

                var existingCategories = await _context.ItemCategories.Where(ic => ic.ItemId == item.Id).ToArrayAsync(cancellationToken);
                var existingRates= await _context.ItemRates.Where(ic => ic.ItemId == item.Id).ToArrayAsync(cancellationToken);
                _context.ItemCategories.RemoveRange(existingCategories);
                _context.ItemRates.RemoveRange(existingRates);

                _fileService.DeleteItemDirectory(item.Id.ToString());
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