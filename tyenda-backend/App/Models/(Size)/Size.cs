using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._Size_
{
    public class Size
    {
        [Key]
        public Guid Id { get; set; }

        public SizeCode? SizeCode { get; set; }
        public int? SizeNumber { get; set; }

        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
    }
}