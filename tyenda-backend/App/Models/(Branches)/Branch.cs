using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._City_;
using tyenda_backend.App.Models._Store_;

namespace tyenda_backend.App.Models._Branches_
{
    public class Branch
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }
        public virtual Store? Store { get; set; }
        
        public Guid CityId { get; set; }
        public virtual City? City { get; set; }

        public string AddressDetails { get; set; } = "";

        public decimal Latitude { get; set; } = 0;
        public decimal Longitude { get; set; } = 0;
    }
}