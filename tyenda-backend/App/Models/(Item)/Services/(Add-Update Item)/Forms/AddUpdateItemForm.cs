using System.Collections.Generic;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._Item_.Services._Add_Update_Item_.Forms
{
    public class AddUpdateItemForm
    {
        public string? Id { get; set; }
        public string Value { get; set; } = "";
        public string Description { get; set; } = "";

        public List<string> Notes { get; set; } = new List<string>();
        public List<string> Categories { get; set; } = new List<string>();
        public decimal Price { get; set; } = 0;
        public int Discount { get; set; } = 0;
        public List<Color>? Colors { get; set; } = new List<Color>();
        public List<Size>? Sizes { get; set; } = new List<Size>();
        public List<ColorSize>? ColorSizes { get; set; } = new List<ColorSize>();
    }

    public class Color
    {
        public string Value { get; set; } = "";
        public int Quantity { get; set; } = 0;
    }

    public class Size
    {
        public SizeCode? Code { get; set; }
        public int? Number { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
    public class ColorSize
    {
        public string Value { get; set; } = "";
        public List<Size> Sizes { get; set; } = new List<Size>();
    }
}