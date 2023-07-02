using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Models._Comment_;
using tyenda_backend.App.Models._ItemCategory_;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Models._ItemImage_;
using tyenda_backend.App.Models._ItemNote_;
using tyenda_backend.App.Models._ItemRate_;
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
            Images = new HashSet<ItemImage>();
            Colors = new HashSet<ItemColor>();
            Orders = new HashSet<Order>();
            Likes = new HashSet<Like>();
            Sizes = new HashSet<Size>();
            Carts = new HashSet<Cart>();
            Notifications = new HashSet<Notification>();
            Categories = new HashSet<ItemCategory>();
            Rates = new HashSet<ItemRate>();
            Comments = new HashSet<Comment>();
            Notes = new HashSet<ItemNote>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid StoreId { get; set; }
        public virtual Store? Store { get; set; }

        public virtual ICollection<ItemImage> Images { get; set; }
        public virtual ICollection<ItemColor> Colors { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<ItemCategory> Categories { get; set; }
        public virtual ICollection<ItemRate> Rates { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ItemNote> Notes { get; set; }
    }
}