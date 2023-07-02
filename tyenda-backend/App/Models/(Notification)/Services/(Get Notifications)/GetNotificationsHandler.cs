using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Notification_.Services._Get_Notifications_
{
    public class GetNotificationsHandler : IRequestHandler<GetNotifications, ICollection<Alert>>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public GetNotificationsHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ICollection<Alert>> Handle(GetNotifications request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim("AccountId");
                var account = await _context.Accounts.SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }

                var alerts = await _context.Alerts
                    .Where(alert => alert.AccountId == account.Id)
                    .OrderByDescending(alert => alert.Notification!.CreatedAt) 
                    .Include(alert => alert.Notification)
                    .ThenInclude(notification => notification!.Item)
                    .ThenInclude(item => item!.Images)
                    .Include(alert => alert.Notification!.Store)
                    .ThenInclude(store => store!.Account)
                    .ToArrayAsync(cancellationToken);

                
                return alerts;

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}