using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using tyenda_backend.App.Models._Branches_;
using tyenda_backend.App.Models._Item_;
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
        }
        
        [Key]
        public Guid Id { get; set; }
        public string OwnerName { get; set; } = "";
        public string OwnerEmail { get; set; } = "";
        public string Website { get; set; } = "";
        public string Description { get; set; } = "";
        public string? BackgroundImage { get; set; } = "";

        public virtual Account? Account { get; set; }
        public Guid AccountId { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<StoreCategory> Categories { get; set; }
        public virtual ICollection<Item> Items { get; set; }

    }
}