using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Category_;
using tyenda_backend.App.Models._Store_;

namespace tyenda_backend.App.Models._Store_Category_
{
    public class StoreCategory
    {
        [Key]
        public Guid StoreId { get; set; }
        public virtual Store? Store { get; set; }

        [Key]
        public Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}