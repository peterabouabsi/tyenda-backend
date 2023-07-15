using System.Collections.Generic;

namespace tyenda_backend.App.Models._Item_.Views
{
    public class ItemEntryView
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public int Stock { get; set; } = 0;
        public double Price { get; set; } = 0;
        public int Discount { get; set; } = 0;
        public double Rate { get; set; } = 0;
        public List<ColorView> Colors { get; set; } = new List<ColorView>();
        public List<SizeView> Sizes { get; set; } = new List<SizeView>();
    }

    public class ColorView
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public int Quantity { get; set; } = 0;
    }

    public class SizeView
    {
        public string Id { get; set; } = "";
        public string? Code { get; set; } = "";
        public int? Number { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
}