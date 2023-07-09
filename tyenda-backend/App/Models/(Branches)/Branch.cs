using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._City_;
using tyenda_backend.App.Models._Store_;

namespace tyenda_backend.App.Models._Branches_
{
    public class Branch
    {
        [Key]
        public Guid StoreId { get; set; }
        public virtual Store? Store { get; set; }
        
        [Key] 
        public Guid CityId { get; set; }
        public virtual City? City { get; set; }

        public string AddressDetails { get; set; } = "";

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}