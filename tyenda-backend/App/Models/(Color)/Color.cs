using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Models._OrderItem_;

namespace tyenda_backend.App.Models._Color_
{
    public class Color
    {
        public Color()
        {
            ItemColors = new HashSet<ItemColor>();
            OrderItems = new HashSet<OrderItem>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";

        public virtual ICollection<ItemColor> ItemColors { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}