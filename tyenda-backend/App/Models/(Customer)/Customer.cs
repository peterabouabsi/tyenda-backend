using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Models._Comment_;
using tyenda_backend.App.Models._Follower_;
using tyenda_backend.App.Models._ItemRate_;
using tyenda_backend.App.Models._Like_;
using tyenda_backend.App.Models._Notification_;
using tyenda_backend.App.Models._Order_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Customer_
{
    public class Customer
    {

        public Customer()
        {
            Orders = new HashSet<Order>();
            Likes = new HashSet<Like>();
            Followers = new HashSet<Follower>();
            Carts = new HashSet<Cart>();
            ItemRates = new HashSet<ItemRate>();
            Comments = new HashSet<Comment>();
            Notifications = new HashSet<Notification>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public bool OnItem { get; set; }
        public bool OnReminder { get; set; }
        public virtual Account? Account { get; set; }
        public Guid AccountId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Follower> Followers { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ItemRate> ItemRates { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}