using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Notification_.Services._View_Notification_
{
    public class ViewNotificationHandler : IRequestHandler<ViewNotification, bool>
    {
        private readonly TyendaContext _context;

        public ViewNotificationHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ViewNotification request, CancellationToken cancellationToken)
        {
            try
            {
                var notificationId = Guid.Parse(request.NotificationId);
                var alert = await _context.Alerts.SingleOrDefaultAsync(alert => alert.NotificationId == notificationId, cancellationToken);
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