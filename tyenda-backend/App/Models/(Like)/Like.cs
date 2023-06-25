using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Item_;

namespace tyenda_backend.App.Models._Like_
{
    public class Like
    {
        [Key]
        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        [Key]
        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}