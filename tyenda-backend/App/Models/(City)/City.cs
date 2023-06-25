using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Branches_;
using tyenda_backend.App.Models._Country_;
using tyenda_backend.App.Models._Order_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._City_
{
    public class City
    {

        public City()
        {
            Branches = new HashSet<Branch>();
            Orders = new HashSet<Order>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";

        public virtual Country? Country { get; set; }
        public Guid CountryId { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}