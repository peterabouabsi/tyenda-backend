using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._ItemCategory_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Add_Update_Item_
{
    public class AddUpdateItemHandler : IRequestHandler<AddUpdateItem, Item>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public AddUpdateItemHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Item> Handle(AddUpdateItem request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);
                if (store == null) throw new Exception("Store not found");

                if (request.AddUpdateItemForm.Id == null)
                {
                    //Add
                    var newItem = new Item()
                    {
                        Id = Guid.NewGuid(),
                        Value = request.AddUpdateItemForm.Value,
                        Description = request.AddUpdateItemForm.Description,
                        StoreId = store.Id,
                        CreatedAt = DateTime.UtcNow,
                        Price = request.AddUpdateItemForm.Price,
                        Discount = request.AddUpdateItemForm.Discount
                    };

                    await _context.Items.AddAsync(newItem, cancellationToken);
                    
                    foreach (var categoryId in request.AddUpdateItemForm.Categories)
                    {
                        var newItemCategory = new ItemCategory()
                        {
                            CategoryId = Guid.Parse(categoryId),
                            ItemId = newItem.Id
                        };
                        await _context.ItemCategories.AddAsync(newItemCategory, cancellationToken);
                    }

                    if (request.AddUpdateItemForm.Colors!.Count > 0)
                    {
                    }
                    
                    if (request.AddUpdateItemForm.Sizes!.Count > 0)
                    {
                    }
                    
                    if (request.AddUpdateItemForm.ColorSizes!.Count > 0)
                    {
                    }
                    
                }
                else
                {
                    //Update
                }
                
                return null;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}