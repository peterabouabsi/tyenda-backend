using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models.Enums;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Get_Top_Customers_
{
    public class GetTopCustomersHandler : IRequestHandler<GetTopCustomers, ICollection<object>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetTopCustomersHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<object>> Handle(GetTopCustomers request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = Guid.Parse(_tokenService.GetHeaderTokenClaim(Constants.AccountId));
                var store = await _context.Stores.SingleOrDefaultAsync(account => account.AccountId == accountId, cancellationToken);
                
                if (store == null) throw new UnauthorizedAccessException("Store not found");

                var customerOrderInfo = await _context.Orders
                    .Where(order => order.Item!.StoreId == store.Id && order.OrderStatus == OrderStatus.Completed)
                    .GroupBy(order => order.CustomerId)
                    .Select(group => new
                    {
                        CustomerId = group.Key,
                        TotalOrders = group.Count()
                    })
                    .Join(_context.Customers,
                        orderInfo => orderInfo.CustomerId,
                        customer => customer.Id,
                        (orderInfo, customer) => new
                        {
                            Firstname = customer.Firstname,
                            Lastname = customer.Lastname,
                            Email = customer.Account!.Email,
                            ProfileImage = customer.Account.ProfileImage,
                            TotalOrders = orderInfo.TotalOrders
                        })
                    .ToArrayAsync(cancellationToken);

                return customerOrderInfo;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}