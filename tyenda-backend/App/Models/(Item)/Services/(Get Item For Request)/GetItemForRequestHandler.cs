using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_For_Request_
{
    public class GetItemForRequestHandler : IRequestHandler<GetItemForRequest, Item>
    {
        private readonly TyendaContext _context;

        public GetItemForRequestHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<Item> Handle(GetItemForRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var itemId = Guid.Parse(request.ItemId);
                var item = await _context.Items
                    .Include(item => item.Images)
                    .Include(item => item.Rates)
                    .Include(item => item.Colors)
                    .ThenInclude(color => color.Color)
                    .Include(item => item.Sizes)
                    .SingleOrDefaultAsync(item => item.Id == itemId, cancellationToken);
                if (item == null)
                {
                    throw new Exception("Item not found");
                }
                
                return item;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}