using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Get_Order_
{
    public class GetOrderHandler : IRequestHandler<GetOrder, Order>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetOrderHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Order> Handle(GetOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }

                var order = await _context.Orders
                    .Include(order => order.Item)
                    .ThenInclude(item => item!.Images)
                    .Include(order => order.Item)
                    .ThenInclude(item => item!.Store)
                    .ThenInclude(item => item!.Account)
                    .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.Color)
                    .Include(order => order.City)
                    .ThenInclude(city => city!.Country)
                    .Include(order => order.Customer)
                    .ThenInclude(city => city!.Account)
                    .SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.OrderId), cancellationToken);
                
                return order!;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}