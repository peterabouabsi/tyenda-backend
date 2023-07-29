using System;

namespace tyenda_backend.App.Models._Notification_.Views
{
    public class ViewModerateAlert
    {
        public string NotificationId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Link { get; set; } = "";
        public string ItemImageUrl { get; set; } = "";
        public string ProfileImageUrl { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public bool IsViewed { get; set; } = false;
    }
}