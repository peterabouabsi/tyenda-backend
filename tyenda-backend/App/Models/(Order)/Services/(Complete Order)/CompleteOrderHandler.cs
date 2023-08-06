using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Complete_Order_
{
    public class CompleteOrderHandler : IRequestHandler<CompleteOrder, Order>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public CompleteOrderHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Order> Handle(CompleteOrder request, CancellationToken cancellationToken)
        {
            try
            { 
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                
                var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);
                if (store == null) throw new Exception("Store not found");

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
                    .Include(order => order.Feedbacks.OrderByDescending(feedback => feedback.CreatedAt))
                    .ThenInclude(feedback => feedback.Customer)
                    .ThenInclude(orderCustomer => orderCustomer!.Account)
                    .SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.CompleteOrderForm.OrderId), cancellationToken);

                if (order == null) throw new Exception("Order not found");
                if (order.Item!.StoreId != store.Id) throw new Exception("You don't have permission on this order.");
                
                if (order.OrderStatus == OrderStatus.OnGoing)
                { 
                    order.OrderStatus = OrderStatus.Completed;
                }
                else
                {
                    throw new Exception("Only Submitted orders can be approved or rejected");
                }
                
                order.UpdatedAt = DateTime.UtcNow;
                
                await Task.FromResult(_context.Orders.Update(order));
                await _context.SaveChangesAsync(cancellationToken);
                return order;
            
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}