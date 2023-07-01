namespace tyenda_backend.App.Models._Cart_.Views
{
    public class CartItemModerateView
    {
        public string Id { get; set; } = "";
        public string ItemId { get; set; } = "";
        public string ItemImage { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int Discount { get; set; } = 0;
        public double Price { get; set; } = 0;
        public double Rate { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public string ProfileImage { get; set; } = "";
        public string StoreName { get; set; } = "";
        public int LikesCount { get; set; } = 0;
        public int OrdersCount { get; set; } = 0;
        public string Description { get; set; } = "";
    }
}