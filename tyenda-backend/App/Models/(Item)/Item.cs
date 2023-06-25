using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Models._ItemImage_;
using tyenda_backend.App.Models._Like_;
using tyenda_backend.App.Models._Notification_;
using tyenda_backend.App.Models._Order_;
using tyenda_backend.App.Models._Size_;
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
            Likes = new HashSet<Like>();
            Sizes = new HashSet<Size>();
            Carts = new HashSet<Cart>();
            Notifications = new HashSet<Notification>();   
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
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}