using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Notification_;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Notification_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Approve_Reject_Order_
{
    public class ApproveRejectOrderHandler : IRequestHandler<ApproveRejectOrder, Order>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly INotificationService _notificationService;

        public ApproveRejectOrderHandler(TyendaContext context, ITokenService tokenService, INotificationService notificationService)
        {
            _context = context;
            _tokenService = tokenService;
            _notificationService = notificationService;
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
                    .SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.ApproveRejectOrderForm.OrderId), cancellationToken);

                if (order == null) throw new Exception("Order not found");
                if (order.Item!.StoreId != store.Id) throw new Exception("You don't have permission on this order.");
                
                if (order.OrderStatus == OrderStatus.Submitted)
                {
                    if (request.ApproveRejectOrderForm.IsApproved) 
                        order.OrderStatus = OrderStatus.Approved;

                    if (request.ApproveRejectOrderForm.IsRejected)
                    {
                        order.RejectDescription = request.ApproveRejectOrderForm.RejectDescription;
                        order.OrderStatus = OrderStatus.Rejected;
                    }
                    
                    order.CreatedAt = order.CreatedAt.ToUniversalTime();
                    order.UpdatedAt = DateTime.UtcNow;

                    var notificationMessage = "";
                    if (request.ApproveRejectOrderForm.IsApproved) notificationMessage = "<p>Your order <b>" + order.Reference + "</b> has been approved. Please make sure to confirm the information in order to start working on your request</p>";
                    if (request.ApproveRejectOrderForm.IsRejected) notificationMessage = "<p>Your order <b>" + order.Reference + "</b> has been rejected. Please check the rejection reason</p>";

                    var newNotification = new Notification()
                    {
                        Id = Guid.NewGuid(),
                        Title = "<p><b>Checkout Order</b></p>",
                        Description = notificationMessage,
                        CreatedAt = DateTime.UtcNow,
                        StoreId = order.Item.StoreId,
                        Link = "/application/customer/order/"+order.Id
                    };

                    var newAlert = new Alert()
                    {
                        NotificationId = newNotification.Id,
                        AccountId = order.Customer!.AccountId 
                    };

                    await _context.Notifications.AddAsync(newNotification, cancellationToken);
                    await _context.Alerts.AddAsync(newAlert, cancellationToken);

                    var message = "";
                    if (request.ApproveRejectOrderForm.IsApproved) message = "Your order " + order.Reference + " has been approved. Please make sure to confirm the information in order to start working on your request";
                    if (request.ApproveRejectOrderForm.IsRejected) message = "Your order " + order.Reference + " has been rejected. Please check the rejection reason";

                    if (order.Customer.Account!.OnOrder)
                    {
                        var title = "Checkout Order";
                        var route = "/application/customer/order/" + order.Id;
                        _notificationService.SendNotificationAsync(order.Customer!.AccountId.ToString(), title, message, route);
                    }
                }
                else
                {
                    throw new Exception("Only Submitted orders can be approved or rejected");
                }
                
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