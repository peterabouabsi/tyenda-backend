using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Color_;
using tyenda_backend.App.Models._Item_;

namespace tyenda_backend.App.Models._ItemColor_
{
    public class ItemColor
    {
        [Key]
        public Guid ItemId { get; set; }
        public Item? Item { get; set; }
        
        [Key]
        public Guid ColorId { get; set; }
        public Color? Color { get; set; }

        public int Quantity { get; set; }
    }
}