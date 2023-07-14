using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._City_;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._Order_
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string Reference { get; set; } = "";
        public string Note { get; set; } = "";
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Receiver { get; set; } = "";
        public int Quantity { get; set; }
        public string AddressDetails { get; set; } = "";
        public decimal Latitude {get; set;}
        public decimal Longitude {get; set;}
        public string? ReceiverEmail { get; set; } = "";
        public string? ReceiverPhone { get; set; } = "";
        public Guid CityId { get; set; }
        public virtual City? City { get; set; }

        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
    }
}