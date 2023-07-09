namespace tyenda_backend.App.Models._Store_.Views
{
    public class StoreTopItemBasicView
    {
        public string Id { get; set; } = "";
        public string Value { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Discount { get; set; } = "";
        public double Rate { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public int CountOrders { get; set; } = 0;
        public int CountLikes { get; set; } = 0;
    }
}