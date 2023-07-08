using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Store_.Services._Get_Store_Name_
{
    public class GetStoreNameHandler : IRequestHandler<GetStoreName, string>
    {
        private readonly TyendaContext _context;

        public GetStoreNameHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetStoreName request, CancellationToken cancellationToken)
        {
            try
            {
                var store = await _context.Stores.SingleOrDefaultAsync(store => store.Id == Guid.Parse(request.StoreId),cancellationToken);
                if (store == null)
                {
                    throw new Exception("Store not found");
                }

                return store.Name;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}