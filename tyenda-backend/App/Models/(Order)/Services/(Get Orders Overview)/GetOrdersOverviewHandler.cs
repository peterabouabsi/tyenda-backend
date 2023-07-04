using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Order_.Views;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Get_Orders_Overview_
{
    public class GetOrdersOverviewHandler : IRequestHandler<GetOrdersOverview, ICollection<OrderCountView>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetOrdersOverviewHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<OrderCountView>> Handle(GetOrdersOverview request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var orders = await _context.Orders
                    .Where(order => order.CustomerId == customer.Id)
                    .GroupBy(order => order.OrderStatus)
                    .Select(group => new OrderCountView()
                    {
                        Status = group.Key.ToString(),
                        Count = group.Count()
                    })
                    .ToArrayAsync(cancellationToken);

                return orders;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}