using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models._Store_;

namespace tyenda_backend.App.Models._Notification_
{
    public class Notification
    {

        public Notification()
        {
            Alerts = new HashSet<Alert>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Link { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public Guid? ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        public Guid? StoreId { get; set; }
        public virtual Store? Store { get; set; }

        public Guid? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; }
    }
}