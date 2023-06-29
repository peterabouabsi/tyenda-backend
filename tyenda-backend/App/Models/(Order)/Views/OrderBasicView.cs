using System;

namespace tyenda_backend.App.Models._Order_.Views
{
    public class OrderBasicView
    {
        public string Id { get; set; } = "";
        public string Reference { get; set; } = "";
        public string ProfileImage { get; set; } = "";
        public string StoreName { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int Quantity { get; set; } = 0;
        public double Price { get; set; } = 0;
        public string CustomerName { get; set; } = "";
        public string Receiver { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string OrderStatus { get; set; } = "";
    }
}