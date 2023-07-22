using System.Collections.Generic;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._Order_.Services._Request_Order_.Forms
{
    public class RequestOrderForm
    {
        public string ItemId { get; set; } = "";
        public string ReceiverName { get; set; } = "";
        public string ReceiverEmail { get; set; } = "";
        public string ReceiverPhone { get; set; } = "";
        public string CityId { get; set; } = "";
        public string AddressDetails { get; set; } = "";
        public string Note { get; set; } = "";
        public decimal Longitude { get; set; } = 0;
        public decimal Latitude { get; set; } = 0;
        public List<ColorForm> Colors { get; set; } = new List<ColorForm>();
        public List<SizeForm> Sizes { get; set; } = new List<SizeForm>();
        public List<ColorSizeForm> ColorSizes { get; set; } = new List<ColorSizeForm>();
    }

    public class ColorForm
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public int Quantity { get; set; } = 0;
    }
    
    public class SizeForm
    {
        public SizeCode Code { get; set; }
        public int Number { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
    
    public class ColorSizeForm
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public List<SizeForm> Sizes { get; set; } = new List<SizeForm>();
    }
        
}