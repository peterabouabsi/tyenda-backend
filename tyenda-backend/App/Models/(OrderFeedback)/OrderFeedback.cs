using System;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Order_;

namespace tyenda_backend.App.Models._OrderFeedback_
{
    public class OrderFeedback
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}