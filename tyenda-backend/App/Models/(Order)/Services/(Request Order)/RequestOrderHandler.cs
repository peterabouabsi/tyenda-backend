﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._OrderItem_;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Request_Order_
{
    public class RequestOrderHandler : IRequestHandler<RequestOrder, Guid>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public RequestOrderHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Guid> Handle(RequestOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);
                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

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
                    RejectDescription = null,
                    UpdatedAt = DateTime.UtcNow,
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

                if (sizes!.Count > 0)
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

                if (colorSizes!.Count > 0)
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