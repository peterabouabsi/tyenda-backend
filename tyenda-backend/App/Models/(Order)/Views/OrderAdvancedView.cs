using System.Collections.Generic;

namespace tyenda_backend.App.Models._Order_.Views
{
    public class OrderAdvancedView
    {
        public string Id { get; set; } = "";
        public string ItemId { get; set; } = "";
        public string ItemImage { get; set; } = "";
        public string ItemName { get; set; } = "";
        public string OrderStatus { get; set; } = "";
        public string RejectDescription { get; set; } = "";
        public string StoreName { get; set; } = "";
        public string StoreProfileImage { get; set; } = "";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ReceiverName { get; set; } = "";
        public string ReceiverEmail { get; set; } = "";
        public string ReceiverPhone { get; set; } = "";
        public string Reference { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string AddressDetails { get; set; } = "";
        public string Note { get; set; } = "";
        public List<object> Feedbacks { get; set; } = new List<object>();
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public List<object>? Colors { get; set; } = new List<object>();
        public List<object>? Sizes { get; set; } = new List<object>();
        public List<object>? ColorSizes { get; set; } = new List<object>();
    }
}