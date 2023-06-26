using MediatR;

namespace tyenda_backend.App.Models._Notification_.Services._View_Notification_
{
    public class ViewNotification : IRequest<bool>
    {
        public string NotificationId { get; set; }
     
        public ViewNotification(string notificationId)
        {
            NotificationId = notificationId;
        }

    }
}