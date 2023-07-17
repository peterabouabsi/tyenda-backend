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
        public List<AdvancedSizeView> Sizes { get; set; } = new List<AdvancedSizeView>();
        public List<ColorWithSizeView> ColorSizes { get; set; } = new List<ColorWithSizeView>();
    }

    public class ColorView
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public int Quantity { get; set; } = 1;
    }

    public class AdvancedSizeView
    {
        public string? Id { get; set; } = "";
        public string? Code { get; set; } = "";
        public int? Number { get; set; } = 0;
        public int Quantity { get; set; } = 1;
    }

    public class ColorWithSizeView
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public List<BasicSizeView> Sizes { get; set; } = new List<BasicSizeView>();
    }
    public class BasicSizeView
    {
        public string? Code { get; set; } = "";
        public int? Number { get; set; } = 0;
        public int Quantity { get; set; } = 1;
    }

}