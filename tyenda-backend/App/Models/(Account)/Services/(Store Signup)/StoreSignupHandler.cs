using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_;
using tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_.Form;
using tyenda_backend.App.Models._Branches_;
using tyenda_backend.App.Models._Store_;
using tyenda_backend.App.Models._Store_Category_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Account_.Services._Store_Signup_
{
    public class StoreSignupHandler : IRequestHandler<StoreSignup, bool>
    {
        private readonly TyendaContext _context;
        private readonly IMediator _mediator;
        
        public StoreSignupHandler(TyendaContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        
        public async Task<bool> Handle(StoreSignup request, CancellationToken cancellationToken)
        {
            try
            {
                var signupForm = request.StoreSignupForm;
                
                var account = await _context.Accounts.SingleOrDefaultAsync(account => 
                        account.Username == signupForm.Username || 
                        account.Email == signupForm.Email
                    ,cancellationToken);

                if (account != null)
                {
                    if (account.Username == signupForm.Username)
                    {
                        throw new Exception("Username already exist");   
                    }
                    else
                    {
                        throw new Exception("Email already in use");
                    }
                }

                var storeRole = await _context.Roles.SingleOrDefaultAsync(role => role.Value == "Store", cancellationToken);
                var newAccount = new Account()
                {
                    Id = Guid.NewGuid(),
                    Email = signupForm.Email,
                    Username = signupForm.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(signupForm.Password),
                    CreatedAt = DateTime.UtcNow,
                    ProfileImage = null,
                    PhoneNumber = signupForm.PhoneNumber,
                    OnNovelty = false,
                    RoleId = storeRole!.Id
                };

                var newStore = new Store()
                {
                    Id = Guid.NewGuid(),
                    Website = signupForm.Website,
                    OwnerName = signupForm.OwnerName,
                    OwnerEmail = signupForm.OwnerEmail,
                    BackgroundImage = null,
                    Description = signupForm.Description,
                    AccountId = newAccount.Id
                };

                await _context.Accounts.AddAsync(newAccount, cancellationToken);
                await _context.Stores.AddAsync(newStore, cancellationToken);
                
                foreach (var categoryId in signupForm.CategoryIds)
                {
                    var newStoreCategory = new StoreCategory()
                    {
                        CategoryId = Guid.Parse(categoryId),
                        StoreId = newStore.Id
                    };
                    await _context.StoreCategories.AddAsync(newStoreCategory,cancellationToken);
                }

                foreach (var branch in signupForm.Branches)
                {
                    var newBranch = new Branch()
                    {
                        StoreId = newStore.Id,
                        CityId = Guid.Parse(branch.CityId),
                        AddressDetails = branch.AddressDetails
                    };

                    await _context.Branches.AddAsync(newBranch, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
                
                var sendActivationEmail = new SendActivationEmail(new SendActivationEmailForm(){Email = request.StoreSignupForm.Email});
                await _mediator.Send(sendActivationEmail, cancellationToken);

                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}