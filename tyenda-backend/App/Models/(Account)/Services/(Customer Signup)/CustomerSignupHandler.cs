using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_;
using tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_.Form;
using tyenda_backend.App.Models._Customer_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Account_.Services._Customer_Signup_
{
    public class CustomerSignupHandler : IRequestHandler<CustomerSignup, bool>
    {
        private readonly TyendaContext _context;
        private readonly IMediator _mediator;

        public CustomerSignupHandler(TyendaContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        
        public async Task<bool> Handle(CustomerSignup request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(
                    account => account.Username == request.CustomerSignupForm.Username.Trim() || account.Email == request.CustomerSignupForm.Email
                    ,cancellationToken);

                if (account != null)
                {
                    if (account.Username.Trim() == request.CustomerSignupForm.Username.Trim())
                    {
                        throw new Exception("Username already exist");   
                    }
                    else
                    {
                        throw new Exception("Email already in use");
                    }
                }

                var customerRole = await _context.Roles.SingleOrDefaultAsync(role => role.Value == "Customer", cancellationToken);
                if (customerRole == null)
                {
                    throw new Exception("Role not found");   
                }

                var newAccount = new Account()
                {
                    Id = Guid.NewGuid(),
                    Username = request.CustomerSignupForm.Username.Trim(),
                    Email = request.CustomerSignupForm.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.CustomerSignupForm.Password),
                    RoleId = customerRole.Id,
                    CreatedAt = DateTime.UtcNow,
                    Active = false,
                    PhoneNumber = request.CustomerSignupForm.PhoneNumber!.Trim(),
                    ProfileImage = null
                };

                var newCustomer = new Customer()
                {
                    Id = Guid.NewGuid(),
                    Firstname = request.CustomerSignupForm.Firstname.Trim(),
                    Lastname = request.CustomerSignupForm.Lastname.Trim(),
                    OnItem = false,
                    AccountId = newAccount.Id
                };

                await _context.Accounts.AddAsync(newAccount, cancellationToken);
                await _context.Customers.AddAsync(newCustomer, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var sendActivationEmail = new SendActivationEmail(new SendActivationEmailForm(){Email = request.CustomerSignupForm.Email});
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