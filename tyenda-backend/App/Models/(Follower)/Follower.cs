using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Store_;

namespace tyenda_backend.App.Models._Follower_
{
    public class Follower
    {
        [Key]
        public Guid StoreId { get; set; }
        public virtual Store? Store { get; set; }

        [Key]
        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}