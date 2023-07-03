using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._ItemRate_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Rate_Item_
{
    public class RateItemHandler : IRequestHandler<RateItem, object>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public RateItemHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<object> Handle(RateItem request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim("AccountId");
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);

                if (customer == null)
                {
                    throw new UnauthorizedAccessException("Customer not found");
                }

                var customerId = customer.Id;
                var itemId = Guid.Parse(request.Form.ItemId);
                
                var rate = await _context.ItemRates.SingleOrDefaultAsync(itemRate => itemRate.ItemId == itemId && itemRate.CustomerId == customerId,cancellationToken);
                var finalRate = request.Form.Rate;
                if (rate == null)
                {
                    //Add Rate
                    var newRate = new ItemRate()
                    {
                        ItemId = itemId,
                        CustomerId = customerId,
                        Rate = request.Form.Rate
                    };
                    await _context.ItemRates.AddAsync(newRate, cancellationToken);
                }
                else
                {
                    if (rate.Rate == request.Form.Rate)
                    { 
                        //Remove Rate       
                        await Task.FromResult(_context.ItemRates.Remove(rate));
                        finalRate = 0;
                    }
                    else
                    {
                        //Update Rate
                        rate.Rate = request.Form.Rate;
                        await Task.FromResult(_context.ItemRates.Update(rate));
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                
                var newItemRate = await _context.ItemRates
                    .Where(itemRate => itemRate.ItemId == itemId)
                    .AverageAsync(itemRate => itemRate.Rate, cancellationToken);
                
                return new
                {
                    itemRate = newItemRate,
                    myRate = finalRate
                };

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}