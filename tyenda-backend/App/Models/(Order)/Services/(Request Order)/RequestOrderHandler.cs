using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Notification_;
using tyenda_backend.App.Models._OrderItem_;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Notification_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Request_Order_
{
    public class RequestOrderHandler : IRequestHandler<RequestOrder, Guid>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly INotificationService _notificationService;

        public RequestOrderHandler(TyendaContext context, ITokenService tokenService, INotificationService notificationService)
        {
            _context = context;
            _tokenService = tokenService;
            _notificationService = notificationService;
        }

        public async Task<Guid> Handle(RequestOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                
                if (customer == null) throw new UnauthorizedAccessException("Customer not found");
                
                var item = await _context.Items
                    .Include(item => item.Store)
                    .ThenInclude(item => item!.Account)
                    .SingleOrDefaultAsync(item => item.Id == Guid.Parse(request.RequestOrderForm.ItemId), cancellationToken);

                if (item == null) throw new Exception("Item not found");
                
                var newOrder = new Order()
                {
                    Id = Guid.NewGuid(),
                    ItemId = Guid.Parse(request.RequestOrderForm.ItemId),
                    CustomerId = customer.Id,
                    ReceiverName = !string.IsNullOrEmpty(request.RequestOrderForm.ReceiverName.Trim())? request.RequestOrderForm.ReceiverName : null,
                    ReceiverEmail = !string.IsNullOrEmpty(request.RequestOrderForm.ReceiverEmail.Trim())? request.RequestOrderForm.ReceiverEmail : null,
                    ReceiverPhone = !string.IsNullOrEmpty(request.RequestOrderForm.ReceiverPhone.Trim())? request.RequestOrderForm.ReceiverPhone : null,
                    CityId = Guid.Parse(request.RequestOrderForm.CityId),
                    AddressDetails = request.RequestOrderForm.AddressDetails,
                    Note = request.RequestOrderForm.Note,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    RejectDescription = null,
                    Reference = "OR-"+DateTime.Now.Ticks,
                    Latitude = request.RequestOrderForm.Latitude,
                    Longitude = request.RequestOrderForm.Longitude,
                    OrderStatus = OrderStatus.Submitted
                };

                await _context.Orders.AddAsync(newOrder, cancellationToken);

                var colors = request.RequestOrderForm.Colors;
                var sizes = request.RequestOrderForm.Sizes;
                var colorSizes = request.RequestOrderForm.ColorSizes;
                
                if (colors!.Count > 0)
                {
                    foreach (var color in colors)
                    {
                        var newOrderItem = new OrderItem()
                        {
                            Id = Guid.NewGuid(),
                            ColorId = Guid.Parse(color.Id),
                            OrderId = newOrder.Id,
                            SizeCode = null,
                            SizeNumber = null,
                            ItemId = Guid.Parse(request.RequestOrderForm.ItemId),
                            Quantity = color.Quantity
                        };
                        await _context.OrderItems.AddAsync(newOrderItem, cancellationToken);
                    }
                }
                else if (sizes!.Count > 0)
                {
                    foreach (var size in sizes)
                    {
                        var newOrderItem = new OrderItem()
                        {
                            Id = Guid.NewGuid(),
                            ColorId = null,
                            OrderId = newOrder.Id,
                            SizeCode = size.Code,
                            SizeNumber = size.Number,
                            ItemId = Guid.Parse(request.RequestOrderForm.ItemId),
                            Quantity = size.Quantity
                        };
                        await _context.OrderItems.AddAsync(newOrderItem, cancellationToken);
                    }
                }
                else if (colorSizes!.Count > 0)
                {
                    foreach(var colorSize in colorSizes)
                    {
                        foreach(var size in colorSize.Sizes)
                        {
                            var newOrderItem = new OrderItem()
                            {
                                Id = Guid.NewGuid(),
                                ColorId = Guid.Parse(colorSize.Id),
                                OrderId = newOrder.Id,
                                SizeCode = size.Code,
                                SizeNumber = size.Number,
                                ItemId = Guid.Parse(request.RequestOrderForm.ItemId),
                                Quantity = size.Quantity
                            };   
                            await _context.OrderItems.AddAsync(newOrderItem, cancellationToken);
                        }
                    }
                }
                else
                {
                    var newOrderItem = new OrderItem()
                    {
                        Id = Guid.NewGuid(),
                        ColorId = null,
                        OrderId = newOrder.Id,
                        SizeCode = null,
                        SizeNumber = null,
                        ItemId = Guid.Parse(request.RequestOrderForm.ItemId),
                        Quantity = request.RequestOrderForm.Quantity
                    };
                    await _context.OrderItems.AddAsync(newOrderItem, cancellationToken);
                }
                
                var newNotification = new Notification()
                {
                    Id = Guid.NewGuid(),
                    Title = "<p><b>New Order Requested</b></p>",
                    Description = "<p>You have just received a new order from " + customer.Firstname + " " +
                                  customer.Lastname + ". Click to see order’s details.</p>",
                    CreatedAt = DateTime.UtcNow,
                    CustomerId = customer.Id,
                    Link = "/application/store/order/"+newOrder.Id
                };
                
                var newAlert = new Alert()
                {
                    NotificationId = newNotification.Id,
                    AccountId = item.Store!.AccountId
                };

                await _context.Notifications.AddAsync(newNotification, cancellationToken);
                await _context.Alerts.AddAsync(newAlert, cancellationToken);
                
                //Send Notification to store
                if (item.Store!.Account!.OnOrder)
                {
                    string title = "New Order Requested";
                    string message = "You have just received a new order from " + customer.Firstname + " " + customer.Lastname + ". Click to see order’s details.";
                    string route = "/application/store/order/"+newOrder.Id;
                    _notificationService.SendNotificationAsync(item.Store.AccountId.ToString(), title, message, route);
                }
                await _context.SaveChangesAsync(cancellationToken);

                return newOrder.Id;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}