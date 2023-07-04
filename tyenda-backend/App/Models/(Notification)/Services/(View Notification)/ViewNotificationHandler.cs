using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Notification_.Services._View_Notification_
{
    public class ViewNotificationHandler : IRequestHandler<ViewNotification, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public ViewNotificationHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(ViewNotification request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = Guid.Parse(_tokenService.GetHeaderTokenClaim(Constants.AccountId));
                var notificationId = Guid.Parse(request.NotificationId);
                var alert = await _context.Alerts.SingleOrDefaultAsync(alert => alert.NotificationId == notificationId && alert.AccountId == accountId, cancellationToken);
                if (alert == null)
                {
                    throw new Exception("Alert not found");
                }

                alert.IsViewed = true;
                await Task.FromResult(_context.Alerts.Update(alert));
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