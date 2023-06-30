namespace tyenda_backend.App.Models._Item_.Views
{
    public class ItemBasicView
    {
        public string Id { get; set; } = "";
        public string ProfileImage { get; set; } = "";
        public string StoreName { get; set; } = "";
        public string StoreEmail { get; set; } = "";
        public string Value { get; set; } = "";
        public double Price { get; set; } = 0;
        public int Discount { get; set; } = 0;
        public double Rate { get; set; } = 0;
        public string ItemImage { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsAddedToCart { get; set; } = false;
        public bool IsItemLiked { get; set; } = false;
    }
}