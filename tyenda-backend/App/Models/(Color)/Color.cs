using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._ItemColor_;

namespace tyenda_backend.App.Models._Color_
{
    public class Color
    {
        public Color()
        {
            ItemColors = new HashSet<ItemColor>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";

        public virtual ICollection<ItemColor> ItemColors { get; set; }
    }
}