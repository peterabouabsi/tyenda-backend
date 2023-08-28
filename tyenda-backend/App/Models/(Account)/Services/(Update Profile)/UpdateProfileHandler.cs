using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Store_Category_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Update_Profile_
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfile, object>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public UpdateProfileHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(UpdateProfile request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .Include(account => account.Customer)
                    .Include(account => account.Store)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }

                var profileData = new object();
                if (account.Role!.Value == Constants.CustomerRole)
                {
                    var customer = account.Customer;
                    if (customer == null)
                    {
                        throw new UnauthorizedAccessException("Customer not found");
                    }
                    
                    account.Username = request.UpdateProfileForm!.UpdateCustomerForm!.Username;
                    account.PhoneNumber = request.UpdateProfileForm!.UpdateCustomerForm!.Phone;
                    account.Email = request.UpdateProfileForm!.UpdateCustomerForm!.Email;
                    account.OnOrder = request.UpdateProfileForm!.UpdateCustomerForm!.OnOrder;

                    customer.Firstname = request.UpdateProfileForm!.UpdateCustomerForm.Firstname;
                    customer.Lastname = request.UpdateProfileForm!.UpdateCustomerForm.Lastname;
                    customer.OnItem = request.UpdateProfileForm!.UpdateCustomerForm.OnItem;

                    profileData = new
                    {
                        Firstname = account.Customer!.Firstname,
                        Lastname = account.Customer!.Lastname,
                        Username = account.Username,
                        Email = account.Email,
                        Phone = account.PhoneNumber,
                        ProfileImage = account.ProfileImage,
                        OnItem = account.Customer.OnItem,
                        OnOrder = account.OnOrder
                    };
                }

                if (account.Role!.Value == Constants.StoreRole)
                {
                    var store = account.Store;
                    if (store == null)
                    {
                        throw new UnauthorizedAccessException("Store not found");
                    }

                    store.Name = request.UpdateProfileForm!.UpdateStoreForm!.Name;
                    store.Website = request.UpdateProfileForm!.UpdateStoreForm!.Website;
                    store.OwnerName = request.UpdateProfileForm!.UpdateStoreForm!.OwnerName;
                    store.OwnerEmail = request.UpdateProfileForm!.UpdateStoreForm!.OwnerEmail;
                    store.Description = request.UpdateProfileForm!.UpdateStoreForm!.Description;

                    account.Email = request.UpdateProfileForm!.UpdateStoreForm!.Email;
                    account.PhoneNumber = request.UpdateProfileForm!.UpdateStoreForm!.Phone;

                    var existingCategories = await _context.StoreCategories.Where(ic => ic.StoreId == store.Id).ToArrayAsync(cancellationToken);
                    _context.StoreCategories.RemoveRange(existingCategories);
                    foreach (var categoryId in request.UpdateProfileForm.UpdateStoreForm.CategoryIds)
                    {
                        var newStoreCategory = new StoreCategory()
                        {
                            CategoryId = Guid.Parse(categoryId),
                            StoreId = store.Id
                        };
                        await _context.StoreCategories.AddAsync(newStoreCategory, cancellationToken);
                    }
                    
                    profileData = new
                    {
                        OwnerName = store.OwnerName,
                        OwnerEmail = store.OwnerEmail,
                        Name = store.Name,
                        Username = account.Username,
                        Email = account.Email,
                        Phone = account.PhoneNumber,
                        ProfileImage = account.ProfileImage,
                        BackgroundImage = store.BackgroundImage,
                        OnOrder = account.OnOrder
                    };

                }
                
                await _context.SaveChangesAsync(cancellationToken);
                return profileData;

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}