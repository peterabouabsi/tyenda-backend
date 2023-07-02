using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Item_;

namespace tyenda_backend.App.Models._ItemNote_
{
    public class ItemNote
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";
        
        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
    }
}