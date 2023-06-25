using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Models._ItemImage_;
using tyenda_backend.App.Models._Order_;
using tyenda_backend.App.Models._Store_;

namespace tyenda_backend.App.Models._Item_
{
    public class Item
    {
        public Item()
        {
            ItemImages = new HashSet<ItemImage>();
            ItemColors = new HashSet<ItemColor>();
            Orders = new HashSet<Order>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid StoreId { get; set; }
        public virtual Store? Store { get; set; }

        public virtual ICollection<ItemImage> ItemImages { get; set; }
        public virtual ICollection<ItemColor> ItemColors { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}