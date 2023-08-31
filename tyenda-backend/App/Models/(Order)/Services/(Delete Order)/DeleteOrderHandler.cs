using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Models._OrderItem_;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Delete_Order_
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrder, Order>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public DeleteOrderHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Order> Handle(DeleteOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts.Include(account => account.Role).SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null) throw new Exception("Account not found");
                
                var accountRole = account.Role!.Value;

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
                    .SingleOrDefaultAsync(order => order.Id == Guid.Parse(request.OrderId), cancellationToken);
                
                if (order == null) throw new Exception("Order not found");

                if (accountRole == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                    if (customer == null) throw new Exception("Customer not found");
                    
                    if (order.CustomerId == customer.Id)
                    {
                        await Task.FromResult(_context.Orders.Remove(order));   
                    }
                    else
                    {
                        throw new Exception("You don't own this order");
                    }
                }

                if (accountRole == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);

                    if (store == null) throw new Exception("Store not found");
                    
                    if (order.Item!.StoreId == store.Id)
                    {
                        await Task.FromResult(_context.Orders.Remove(order));
                    }
                    else
                    {
                        throw new Exception("You don't own this order");
                    }
                }
                
                var itemColors = await _context.ItemColors.Where(ic => ic.ItemId == order.ItemId).ToArrayAsync(cancellationToken);
                
                /*When Order is ongoing and deleted */
                if (order.OrderStatus == OrderStatus.OnGoing)
                {
                    //Add quantities to Item
                    if (itemColors.Length == 0)
                    {
                        var orderItem = order.OrderItems.First();
                        //If Quantity only
                        if (String.IsNullOrEmpty(orderItem.ColorId.ToString()) && String.IsNullOrEmpty(orderItem.SizeCode.ToString()) && orderItem.SizeNumber == null)
                        {
                            order.Item!.Quantity += orderItem.Quantity;
                            await Task.FromResult(_context.Items.Update(order.Item));
                        }
                    }
                    //Add quantities to ItemColors Table
                    else
                    {
                        foreach (var orderItem in order.OrderItems)
                        {
                            var itemColor = new ItemColor();
                            if (!String.IsNullOrEmpty(orderItem.ColorId.ToString()) && String.IsNullOrEmpty(orderItem.SizeCode.ToString()) && orderItem.SizeNumber == null)
                            {
                                itemColor = itemColors.SingleOrDefault(ic => ic.ColorId == orderItem.OrderId);
                            }
                            if (String.IsNullOrEmpty(orderItem.ColorId.ToString()) && !String.IsNullOrEmpty(orderItem.SizeCode.ToString()) && orderItem.SizeNumber == null)
                            {
                                itemColor = itemColors.SingleOrDefault(ic => ic.SizeCode == orderItem.SizeCode);
                            }
                            if (!String.IsNullOrEmpty(orderItem.ColorId.ToString()) && !String.IsNullOrEmpty(orderItem.SizeCode.ToString()) && orderItem.SizeNumber == null)
                            {
                                itemColor = itemColors.SingleOrDefault(ic => ic.ColorId == orderItem.ColorId && ic.SizeCode == orderItem.SizeCode);
                            }
                            if (!String.IsNullOrEmpty(orderItem.ColorId.ToString()) && String.IsNullOrEmpty(orderItem.SizeCode.ToString()) && orderItem.SizeNumber != null)
                            {
                                itemColor = itemColors.SingleOrDefault(ic => ic.ColorId == orderItem.ColorId && ic.SizeNumber == orderItem.SizeNumber);
                            }
                            
                            if (itemColor == null)
                            {
                                var newItemColor = new ItemColor()
                                {
                                    Id = Guid.NewGuid(),
                                    ColorId = orderItem.ColorId,
                                    ItemId = orderItem.ItemId,
                                    SizeCode = orderItem.SizeCode,
                                    SizeNumber = orderItem.SizeNumber,
                                    Quantity = orderItem.Quantity
                                };
                                await _context.ItemColors.AddAsync(newItemColor, cancellationToken);
                            }
                            else
                            {
                                itemColor.Quantity += orderItem.Quantity;
                                await Task.FromResult(_context.ItemColors.Update(itemColor));
                            }
                        }

                        await _context.SaveChangesAsync(cancellationToken);
                    }
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