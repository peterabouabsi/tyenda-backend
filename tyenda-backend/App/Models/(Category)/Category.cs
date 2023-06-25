using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Store_Category_;

namespace tyenda_backend.App.Models._Category_
{
    public class Category
    {

        public Category()
        {
            Stores = new HashSet<StoreCategory>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";

        public virtual ICollection<StoreCategory> Stores { get; set; }
    }
}