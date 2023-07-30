using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._OrderFeedback_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Add_Feedback_
{
    public class AddFeedbackHandler : IRequestHandler<AddFeedback, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public AddFeedbackHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(AddFeedback request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId),cancellationToken);
                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var isOrderForCustomer = await _context.Orders.AnyAsync(order => order.Id == Guid.Parse(request.AddFeedbackForm.OrderId) && order.CustomerId == customer.Id, cancellationToken: cancellationToken);
                if (isOrderForCustomer == false)
                {
                    throw new Exception("Unable to add feedback");
                }

                var newFeedback = new OrderFeedback()
                {
                    Id = Guid.NewGuid(),
                    Value = request.AddFeedbackForm.Feedback,
                    CreatedAt = DateTime.UtcNow,
                    CustomerId = customer.Id,
                    OrderId = Guid.Parse(request.AddFeedbackForm.OrderId)
                };

                await _context.OrderFeedbacks.AddAsync(newFeedback, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}