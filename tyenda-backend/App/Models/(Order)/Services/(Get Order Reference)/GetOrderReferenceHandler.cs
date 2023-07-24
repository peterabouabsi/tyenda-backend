using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Order_.Services._Get_Order_Reference_
{
    public class GetOrderReferenceHandler : IRequestHandler<GetOrderReference, string>
    {
        private readonly TyendaContext _context;

        public GetOrderReferenceHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetOrderReference request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _context.Orders.SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.orderId),
                cancellationToken);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                return order.Reference;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}