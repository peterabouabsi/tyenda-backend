﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Color_;
using tyenda_backend.App.Models._ItemCategory_;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Models._ItemNote_;
using tyenda_backend.App.Models._Notification_;
using tyenda_backend.App.Models._Size_;
using tyenda_backend.App.Services.Notification_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Add_Update_Item_
{
    public class AddUpdateItemHandler : IRequestHandler<AddUpdateItem, Item>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly INotificationService _notificationService;

        public AddUpdateItemHandler(TyendaContext context, ITokenService tokenService, INotificationService notificationService)
        {
            _context = context;
            _tokenService = tokenService;
            _notificationService = notificationService;
        }

        public async Task<Item> Handle(AddUpdateItem request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == Guid.Parse(accountId), cancellationToken);
                if (store == null) throw new Exception("Store not found");

                if (request.AddUpdateItemForm.Id == null)
                {
                    //Add
                    var newItem = new Item()
                    {
                        Id = Guid.NewGuid(),
                        Value = request.AddUpdateItemForm.Value,
                        Description = request.AddUpdateItemForm.Description,
                        StoreId = store.Id,
                        CreatedAt = DateTime.UtcNow,
                        Price = request.AddUpdateItemForm.Price,
                        Discount = request.AddUpdateItemForm.Discount
                    };

                    await _context.Items.AddAsync(newItem, cancellationToken);
                    
                    foreach (var categoryId in request.AddUpdateItemForm.Categories)
                    {
                        var newItemCategory = new ItemCategory()
                        {
                            CategoryId = Guid.Parse(categoryId),
                            ItemId = newItem.Id
                        };
                        await _context.ItemCategories.AddAsync(newItemCategory, cancellationToken);
                    }
                    foreach (var note in request.AddUpdateItemForm.Notes)
                    {
                        var newNote = new ItemNote()
                        {
                            Id = Guid.NewGuid(),
                            Value = note,
                            ItemId = newItem.Id
                        };
                        await _context.ItemNotes.AddAsync(newNote, cancellationToken);
                    }

                    if (request.AddUpdateItemForm.Colors!.Count > 0)
                    {
                        foreach (var colorForm in request.AddUpdateItemForm.Colors)
                        {
                            var color = await _context.Colors.SingleOrDefaultAsync(color => color.Value == colorForm.Value, cancellationToken);
                            
                            var newColor = new Color() {Id = Guid.NewGuid(), Value = colorForm.Value};
                            if (color == null)
                            {
                                await _context.Colors.AddAsync(newColor, cancellationToken);
                            }

                            var newItemColor = new ItemColor()
                            {
                                Id = Guid.NewGuid(),
                                ColorId = color?.Id ?? newColor.Id,
                                ItemId = newItem.Id,
                                Quantity = colorForm.Quantity
                            };

                            await _context.ItemColors.AddAsync(newItemColor, cancellationToken);
                        }
                    }
                    else if (request.AddUpdateItemForm.Sizes!.Count > 0)
                    {
                        foreach (var sizeForm in request.AddUpdateItemForm.Sizes)
                        {
                            var newItemSize = new Size()
                            {
                                Id = Guid.NewGuid(),
                                ItemId = newItem.Id,
                                Quantity = sizeForm.Quantity,
                                SizeCode = sizeForm.Code,
                                SizeNumber = sizeForm.Number
                            };

                            await _context.Sizes.AddAsync(newItemSize, cancellationToken);
                        }
                    }
                    else if (request.AddUpdateItemForm.ColorSizes!.Count > 0)
                    {
                        foreach (var colorForm in request.AddUpdateItemForm.ColorSizes)
                        {
                            var color = await _context.Colors.SingleOrDefaultAsync(color => color.Value == colorForm.Value, cancellationToken);
                            
                            var newColor = new Color() {Id = Guid.NewGuid(), Value = colorForm.Value};
                            if (color == null)
                            {
                                await _context.Colors.AddAsync(newColor, cancellationToken);
                            }

                            foreach (var sizeForm in colorForm.Sizes)
                            {

                                var newItemColor = new ItemColor()
                                {
                                    Id = Guid.NewGuid(),
                                    ItemId = newItem.Id,
                                    ColorId = color?.Id ?? newColor.Id,
                                    Quantity = sizeForm.Quantity,
                                    SizeCode = sizeForm.Code,
                                    SizeNumber = sizeForm.Number
                                };

                                await _context.ItemColors.AddAsync(newItemColor, cancellationToken);
                            }
 
                        }
                    }
                    else newItem.Quantity = request.AddUpdateItemForm.Quantity;

                    //Send notification to customers (OnItem = true, follow the store)
                    var recipients = await _context.Followers
                        .Where(follower => follower.StoreId == store.Id)
                        .Include(follower => follower.Customer)
                        .ToArrayAsync(cancellationToken);

                    var newNotification = new Notification()
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        Title = "<p><b>" + newItem.Value + "</b> from <b>" + store.Name + "</b> is out here!!!<p>",
                        Description = "We\'ve just added a fantastic new product to our store. Introducing " + newItem.Value + "! is now available for purchase. Don't miss out – come and check it out in our store today!",
                        ItemId = newItem.Id,
                        StoreId = store.Id,
                        Link = "/application/customer/item/" + newItem.Id
                    };

                    await _context.Notifications.AddAsync(newNotification, cancellationToken);

                    foreach (var recipient in recipients)
                    {
                        if(recipient.Customer != null ){
                            var newAlert = new Alert()
                            {
                                NotificationId = newNotification.Id,
                                AccountId = recipient.Customer.AccountId,
                                IsViewed = false
                            };
                            await _context.Alerts.AddAsync(newAlert, cancellationToken);
                            
                            if ( recipient.Customer.OnItem)
                            {
                                //Send notification
                                var title = newItem.Value+" from "+store.Name+" is out here!!!";
                                var message = "We\'ve just added a fantastic new product to our store. Introducing "+newItem.Value+"! is now available for purchase. Don't miss out – come and check it out in our store today!";
                                var route = "/application/customer/item/" + newItem.Id;
                                _notificationService.SendNotificationAsync(recipient.Customer.AccountId.ToString(), title, message, route);
                            }
                        }
                        
                    }
                    
                    await _context.SaveChangesAsync(cancellationToken);
                    return newItem;
                }
                else
                {
                    //Update
                    var item = await _context.Items.SingleOrDefaultAsync(item => item.Id == Guid.Parse(request.AddUpdateItemForm.Id) && item.StoreId == store.Id , cancellationToken);
                    if (item == null) throw new Exception("Item not found");
                    
                    item.Value = request.AddUpdateItemForm.Value;
                    item.Description = request.AddUpdateItemForm.Description;
                    item.StoreId = store.Id;
                    item.Price = request.AddUpdateItemForm.Price;
                    item.Discount = request.AddUpdateItemForm.Discount;

                    var existingNotes = await _context.ItemNotes.Where(note => note.ItemId == item.Id).ToArrayAsync(cancellationToken);
                    _context.ItemNotes.RemoveRange(existingNotes);
                    foreach (var note in request.AddUpdateItemForm.Notes)
                    {
                        var newNote = new ItemNote()
                        {
                            Id = Guid.NewGuid(),
                            Value = note,
                            ItemId = item.Id
                        };
                        await _context.ItemNotes.AddAsync(newNote, cancellationToken);
                    }

                    var existingCategories = await _context.ItemCategories.Where(ic => ic.ItemId == item.Id).ToArrayAsync(cancellationToken);
                    _context.ItemCategories.RemoveRange(existingCategories);
                    foreach (var categoryId in request.AddUpdateItemForm.Categories)
                    {
                        var newItemCategory = new ItemCategory()
                        {
                            CategoryId = Guid.Parse(categoryId),
                            ItemId = item.Id
                        };
                        await _context.ItemCategories.AddAsync(newItemCategory, cancellationToken);
                    }

                    // Remove existing colors associated with the item
                    var existingColors = _context.ItemColors.Where(ic => ic.ItemId == item.Id);
                    _context.ItemColors.RemoveRange(existingColors);
                    
                    // Remove existing sizes associated with the item
                    var existingSizes = _context.Sizes.Where(s => s.ItemId == item.Id);
                    _context.Sizes.RemoveRange(existingSizes);

                    // Remove existing color sizes associated with the item
                    var existingColorSizes = await _context.ItemColors
                        .Where(ic => ic.ItemId == item.Id)
                        .ToArrayAsync(cancellationToken);
                    _context.ItemColors.RemoveRange(existingColorSizes);

                    if (request.AddUpdateItemForm.Colors!.Count > 0) 
                    { 
                        foreach (var colorForm in request.AddUpdateItemForm.Colors)
    {
        var color = await _context.Colors.SingleOrDefaultAsync(c => c.Value == colorForm.Value, cancellationToken);

        var newColor = new Color() { Id = Guid.NewGuid(), Value = colorForm.Value };
        if (color == null)
        {
            await _context.Colors.AddAsync(newColor, cancellationToken);
        }

        var newItemColor = new ItemColor()
        {
            Id = Guid.NewGuid(),
            ColorId = color?.Id ?? newColor.Id,
            ItemId = item.Id,
            Quantity = colorForm.Quantity
        };

        await _context.ItemColors.AddAsync(newItemColor, cancellationToken);
    } 
                    }
                    else if (request.AddUpdateItemForm.Sizes!.Count > 0) { 
                        foreach (var sizeForm in request.AddUpdateItemForm.Sizes)
    {
        var newItemSize = new Size()
        {
            Id = Guid.NewGuid(),
            ItemId = item.Id,
            Quantity = sizeForm.Quantity,
            SizeCode = sizeForm.Code,
            SizeNumber = sizeForm.Number
        };

        await _context.Sizes.AddAsync(newItemSize, cancellationToken);
    } 
                    }
                    else if (request.AddUpdateItemForm.ColorSizes!.Count > 0)
                    {
                        foreach (var colorForm in request.AddUpdateItemForm.ColorSizes)
                        {
                            var color = await _context.Colors.SingleOrDefaultAsync(c => c.Value == colorForm.Value, cancellationToken);

                            var newColor = new Color() {Id = Guid.NewGuid(), Value = colorForm.Value};
                            if (color == null)
                            {
                                await _context.Colors.AddAsync(newColor, cancellationToken);
                            }

                            foreach (var sizeForm in colorForm.Sizes)
                            {
                                var newItemColor = new ItemColor()
                                {
                                    Id = Guid.NewGuid(),
                                    ColorId = color?.Id ?? newColor.Id,
                                    ItemId = item.Id,
                                    Quantity = sizeForm.Quantity,
                                    SizeCode = sizeForm.Code,
                                    SizeNumber = sizeForm.Number
                                };

                                await _context.ItemColors.AddAsync(newItemColor, cancellationToken);
                            }
                        }
                    }
                    else item.Quantity = request.AddUpdateItemForm.Quantity;
                    
                    await _context.SaveChangesAsync(cancellationToken);

                    return item;
                }
                
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}