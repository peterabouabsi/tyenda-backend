using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models._Store_;

namespace tyenda_backend.App.Models._Cart_
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        
        public Guid? ItemId { get; set; }
        public virtual Item? Item { get; set; }
        public int? Quantity { get; set; }

        public Guid? StoreId { get; set; }
        public virtual Store? Store { get; set; }


        
    }
}