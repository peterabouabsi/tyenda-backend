using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using tyenda_backend.App.Models._Branches_;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Models._Follower_;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models._Notification_;
using tyenda_backend.App.Models._Store_Category_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Store_
{
    public class Store
    {

        public Store()
        {
            Branches = new HashSet<Branch>();
            Categories = new HashSet<StoreCategory>();
            Items = new HashSet<Item>();
            Followers = new HashSet<Follower>();
            Carts = new HashSet<Cart>();
            Notifications = new HashSet<Notification>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string OwnerEmail { get; set; } = "";
        public string Website { get; set; } = "";
        public string Description { get; set; } = "";
        public string? BackgroundImage { get; set; } = "";
        public bool OnOrder { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoPosterUrl { get; set; }
        public virtual Account? Account { get; set; }
        public Guid AccountId { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<StoreCategory> Categories { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Follower> Followers { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

    }
}