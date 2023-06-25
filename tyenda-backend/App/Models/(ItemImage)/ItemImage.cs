using System;
using tyenda_backend.App.Models._Item_;

namespace tyenda_backend.App.Models._ItemImage_
{
    public class ItemImage
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = "";
    
        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
    }
}