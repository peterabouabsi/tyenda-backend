using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Color_;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._ItemColor_
{
    public class ItemColor
    {
        [Key] public Guid Id { get; set; }
        
        public Guid ItemId { get; set; }
        public Item? Item { get; set; }
        
        public Guid? ColorId { get; set; }
        public Color? Color { get; set; }

        public SizeCode? SizeCode { get; set; }
        public int? SizeNumber { get; set; }
        public int Quantity { get; set; }
    }
}