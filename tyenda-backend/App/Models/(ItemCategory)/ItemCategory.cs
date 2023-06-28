using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Category_;
using tyenda_backend.App.Models._Item_;

namespace tyenda_backend.App.Models._ItemCategory_
{
    public class ItemCategory
    {
        [Key]
        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        [Key]
        public Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}