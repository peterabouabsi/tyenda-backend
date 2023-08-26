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

namespace tyenda_backend.App.Models._Order_.Services._Confirm_Order_
{
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrder, Order>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly INotificationService _notificationService;

        public ConfirmOrderHandler(TyendaContext context, ITokenService tokenService, INotificationService notificationService)
        {
            _context = context;
            _tokenService = tokenService;
            _notificationService = notificationService;
        }

        public async Task<Order> Handle(ConfirmOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                if (customer == null) throw new Exception("Customer not found");

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
                    .SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.ConfirmOrderForm.OrderId) && order.CustomerId == customer.Id, cancellationToken);
                
                if (order == null) throw new Exception("Order not found");

                order.OrderStatus = OrderStatus.OnGoing;
                

                order.CreatedAt = order.CreatedAt.ToUniversalTime();
                order.UpdatedAt = DateTime.UtcNow;
                    
                await Task.FromResult(_context.Orders.Update(order));

                var title = "<p><b>Order Confirmation</b></p>";
                var message = "<p>Order <b>"+order.Reference+"</b> has been confirmed by customer "+order.Customer!.Firstname+" "+order.Customer.Lastname+" and you can proceed with the order.</p>";
                var newNotification = new Notification()
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Description = message,
                    CustomerId = order.CustomerId,
                    CreatedAt = DateTime.UtcNow,
                    Link = "/application/store/order/"+order.Id
                };

                var newAlert = new Alert()
                {
                    NotificationId = newNotification.Id,
                    AccountId = order.Item!.Store!.AccountId
                };

                await _context.Notifications.AddAsync(newNotification, cancellationToken);
                await _context.Alerts.AddAsync(newAlert, cancellationToken);
                
                if(order.Item!.Store!.Account!.OnOrder)
                {
                    var notificationTitle = "Order Confirmation";
                    var notificationMessage = "Order " + order.Reference + " has been confirmed by customer " +
                                              order.Customer!.Firstname + " " + order.Customer.Lastname +
                                              " and you can proceed with the order.";
                    var route = "/application/store/order/"+order.Id;
                    _notificationService.SendNotificationAsync(order.Item!.Store!.AccountId.ToString(), notificationTitle, notificationMessage, route);
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