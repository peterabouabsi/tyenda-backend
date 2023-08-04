using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
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

                    customer.Firstname = request.UpdateProfileForm!.UpdateCustomerForm.Firstname;
                    customer.Lastname = request.UpdateProfileForm!.UpdateCustomerForm.Lastname;
                    customer.OnItem = request.UpdateProfileForm!.UpdateCustomerForm.OnItem;
                    customer.OnReminder = request.UpdateProfileForm!.UpdateCustomerForm.OnReminder;

                    profileData = new
                    {
                        Firstname = account.Customer!.Firstname,
                        Lastname = account.Customer!.Lastname,
                        Username = account.Username,
                        Email = account.Email,
                        Phone = account.PhoneNumber,
                        ProfileImage = account.ProfileImage,
                        OnItem = account.Customer.OnItem,
                        OnReminder = account.Customer.OnReminder
                    };
                }

                if (account.Role!.Value == Constants.StoreRole)
                {
                    
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