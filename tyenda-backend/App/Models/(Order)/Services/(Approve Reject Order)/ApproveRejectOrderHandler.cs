using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Approve_Reject_Order_
{
    public class ApproveRejectOrderHandler : IRequestHandler<ApproveRejectOrder, Order>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public ApproveRejectOrderHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Order> Handle(ApproveRejectOrder request, CancellationToken cancellationToken)
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
                    .SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.ApproveRejectOrderForm.OrderId) && order.Item!.StoreId == store.Id, cancellationToken);
                if (order == null) throw new Exception("Order not found");

                if (order.OrderStatus == OrderStatus.Submitted)
                {
                    if (request.ApproveRejectOrderForm.IsApproved) order.OrderStatus = OrderStatus.Approved;
                    if (request.ApproveRejectOrderForm.IsRejected) order.OrderStatus = OrderStatus.Rejected;
                }
                else
                {
                    throw new Exception("Only Submitted orders can be approved or rejected");
                }

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