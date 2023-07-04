using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Item_;

namespace tyenda_backend.App.Models._Comment_
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
    }
}