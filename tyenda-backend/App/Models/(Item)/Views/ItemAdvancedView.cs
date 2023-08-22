using System;
using System.Collections.Generic;

namespace tyenda_backend.App.Models._Item_.Views
{
    public class ItemAdvancedView
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string DisplayedImage { get; set; } = "";
        public List<string> OtherImages { get; set; } = new List<string>();
        public string StoreName { get; set; } = "";
        public string StoreImage { get; set; } = "";
        public double Rate { get; set; }
        public int MyRate { get; set; } = 0;
        public int RatersCount { get; set; }
        public int Discount { get; set; }
        public double OldPrice { get; set; }
        public double CurrentPrice { get; set; }
        public List<string> Notes { get; set; } = new List<string>();
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
        public List<string> Sizes { get; set; } = new List<string>();
        public List<ColorSizeView> ColorSizes { get; set; } = new List<ColorSizeView>();
        public int CountLikes { get; set; } = 0;
        public int CountOrders { get; set; } = 0;
        public int CountComments { get; set; } = 0;
        public string Description { get; set; } = "";
        public bool IsAddedToCart { get; set; } = false;
        public bool IsLiked { get; set; } = false;
    }

    public class ColorSizeView
    {
        public string Value { get; set; } = "";
        public List<string> Sizes { get; set; } = new List<string>();
    }
}