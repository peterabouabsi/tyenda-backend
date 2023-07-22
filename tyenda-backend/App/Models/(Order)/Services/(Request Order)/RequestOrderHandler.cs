using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Order_.Services._Request_Order_
{
    public class RequestOrderHandler : IRequestHandler<RequestOrder, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public RequestOrderHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(RequestOrder request, CancellationToken cancellationToken)
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
                    UpdatedAt = DateTime.UtcNow,
                    Reference = "OR-"+DateTime.Now.Ticks,
                    Latitude = request.RequestOrderForm.Latitude,
                    Longitude = request.RequestOrderForm.Longitude,
                    OrderStatus = OrderStatus.Submitted
                };

                await _context.Orders.AddAsync(newOrder, cancellationToken);
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