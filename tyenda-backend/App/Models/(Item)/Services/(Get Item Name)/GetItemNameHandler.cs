using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_Name_
{
    public class GetItemNameHandler : IRequestHandler<GetItemName, string>
    {
        private readonly TyendaContext _context;

        public GetItemNameHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetItemName request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _context.Items.SingleOrDefaultAsync(item => item.Id == Guid.Parse(request.ItemId),cancellationToken);
                if (item == null)
                {
                    throw new Exception("Item not found");
                }

                return item.Value;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}