using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Alert_;

namespace tyenda_backend.App.Models._Notification_.Services._Get_Notifications_
{
    public class GetNotifications : IRequest<ICollection<Alert>>
    {
    }
}