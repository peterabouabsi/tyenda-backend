using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Delete_Image_
{
    public class DeleteImageHandler : IRequestHandler<DeleteImage, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;


        public DeleteImageHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(DeleteImage request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Store)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);

                if (account == null) throw new Exception("Account not found");

                foreach (var imageId in request.ImageIds)
                {
                    var image = await _context.ItemImages
                        .Include(image => image.Item)
                        .SingleOrDefaultAsync(image => image.Id == Guid.Parse(imageId), cancellationToken);
                    
                    if (image == null) throw new Exception("Image not found");
                    if (account.Store!.Id != image.Item!.StoreId) throw new Exception("You don't have the permission to delete this item!");

                    await Task.FromResult(_context.Remove(image));
                }

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