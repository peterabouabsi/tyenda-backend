using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using tyenda_backend.App.Models._Order_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Customer_
{
    public class Customer
    {

        public Customer()
        {
            Orders = new HashSet<Order>();
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
    }
}