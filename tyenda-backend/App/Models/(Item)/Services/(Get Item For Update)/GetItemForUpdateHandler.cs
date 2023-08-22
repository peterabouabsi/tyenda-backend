using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
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
                    Images = item.Images.Select(image => new {Id = image.Id, Url = image.Url})
                };
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}