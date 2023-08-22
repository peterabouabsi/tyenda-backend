using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._ItemImage_;
using tyenda_backend.App.Services.File_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Add_Update_Image_
{
    public class AddUpdateImageHandler : IRequestHandler<AddUpdateImage, ItemImage>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;

        public AddUpdateImageHandler(TyendaContext context, ITokenService tokenService, IFileService fileService)
        {
            _context = context;
            _tokenService = tokenService;
            _fileService = fileService;
        }

        public async Task<ItemImage> Handle(AddUpdateImage request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);
                if (store == null) throw new Exception("Store not found");
                
                var item = await _context.Items.SingleOrDefaultAsync(item => item.Id == Guid.Parse(request.AddUpdateImageForm.ItemId), cancellationToken);
                if (item == null) throw new Exception("Item not found");
                
                ItemImage itemImage = new ItemImage();
                if (request.AddUpdateImageForm.Id == "")
                {
                    //Add
                    itemImage = new ItemImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = "",
                        ItemId = Guid.Parse(request.AddUpdateImageForm.ItemId)
                    };
                    var itemUrl = _fileService.UploadItemImageFile(request.AddUpdateImageForm.File!, "Items", itemImage.Id.ToString(), request.AddUpdateImageForm.ItemId);
                    
                    itemImage.Url = itemUrl;
                    
                    await _context.ItemImages.AddAsync(itemImage, cancellationToken);
                }
                else
                {
                    //update
                    var itemImageCheck = await _context.ItemImages.SingleOrDefaultAsync(image => image.Id == Guid.Parse(request.AddUpdateImageForm.Id!), cancellationToken);
                    if (itemImageCheck == null) throw new Exception("Image not found");

                    var itemUrl = _fileService.UploadItemImageFile(request.AddUpdateImageForm.File, "Items", itemImageCheck.Id.ToString(), request.AddUpdateImageForm.ItemId);

                    itemImageCheck.Url = itemUrl;
                    await Task.FromResult(_context.ItemImages.Update(itemImageCheck));

                    itemImage = itemImageCheck;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return itemImage;

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}