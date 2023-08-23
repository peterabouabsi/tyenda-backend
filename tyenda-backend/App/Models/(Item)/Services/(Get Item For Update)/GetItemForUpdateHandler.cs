using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_For_Update_
{
    public class GetItemForUpdateHandler : IRequestHandler<GetItemForUpdate, object>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetItemForUpdateHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(GetItemForUpdate request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts.SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null) throw new Exception("Account not found");

                var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);
                if (store == null) throw new Exception("Store not found");
                
                var item = await _context.Items
                    .Include(item => item.Categories).ThenInclude(category => category.Category)
                    .Include(item => item.Images)
                    .Include(item => item.Colors).ThenInclude(color => color.Color)
                    .Include(item => item.Sizes)
                    .SingleOrDefaultAsync(item => item.Id == Guid.Parse(request.ItemId) && item.StoreId == store.Id, cancellationToken);
                if (item == null) throw new Exception("Item not found");

                return new
                {
                    Id = item.Id,
                    Value = item.Value,
                    Description = item.Description,
                    Discount = item.Discount,
                    Price = item.Price,
                    Categories = item.Categories.Select(category => new {Id = category.Category!.Id, Value = category.Category.Value}),
                    Images = item.Images.Select(image => new {Id = image.Id, Url = image.Url}),
                    Colors = item.Colors.Where(color => color.SizeNumber == null && color.SizeCode == null).Select(color => new ColorView(){Id = color.Color!.Id.ToString(), Value = color.Color.Value, Quantity = color.Quantity}),
                    Sizes = item.Sizes.Select(size => new AdvancedSizeView(){Id = size.Id.ToString(), Code = size.SizeCode.ToString(), Number = size.SizeNumber, Quantity = size.Quantity}),
                    ColorSizes = GenerateColorsSizesWithQuantity(item.Colors)
                };
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
        
        private static List<ColorWithSizeView> GenerateColorsSizesWithQuantity(ICollection<ItemColor> colors)
        {
            var result = new List<ColorWithSizeView>();
            if (colors.Any(color => color.SizeCode != null || color.SizeNumber != null))
            {
                foreach (var color in colors)
                {
                    var colorRows = result.SingleOrDefault(data => data.Value == color.Color!.Value);
                    if (colorRows == null)
                    {
                        var sizesList = new List<BasicSizeView>();
                        sizesList.Add(new BasicSizeView(){Code = color.SizeCode.ToString(), Number = color.SizeNumber, Quantity = color.Quantity});

                        var newColorSize = new ColorWithSizeView()
                        {
                            Id = color.Color!.Id.ToString(),
                            Value = color.Color!.Value, 
                            Sizes = sizesList
                        };
                
                        result.Add(newColorSize);
                    }
                    else
                    {
                        var newSizeView = new BasicSizeView(){Code = color.SizeCode.ToString(), Number = color.SizeNumber, Quantity = color.Quantity};
                        colorRows.Sizes.Add(newSizeView);
                    }
                }
    
            }
            
            return result;
        }

    }
}