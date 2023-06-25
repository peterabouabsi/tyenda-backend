using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using tyenda_backend.App.Models._Notification_;

namespace tyenda_backend.App.Models._Alert_
{
    public class Alert
    {
        [Key]
        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }
        
        [Key]
        public Guid NotificationId { get; set; }
        public virtual Notification? Notification { get; set; }

        public bool IsViewed { get; set; }
    }
}