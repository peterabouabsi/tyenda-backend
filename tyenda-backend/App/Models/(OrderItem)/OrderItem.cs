using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Color_;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models._Order_;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._OrderItem_
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
        
        public Guid OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public int Quantity { get; set; } = 1;
        
        public Guid? ColorId { get; set; }
        public virtual Color? Color { get; set; }

        
        public SizeCode? SizeCode { get; set; }
        public int? SizeNumber { get; set; }
    }
}