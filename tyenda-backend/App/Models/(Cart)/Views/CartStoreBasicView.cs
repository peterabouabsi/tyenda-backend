namespace tyenda_backend.App.Models._Cart_.Views
{
    public class CartStoreBasicView
    {
        public string Id { get; set; } = "";
        public string StoreId { get; set; } = "";
        public string ProfileImage { get; set; } = "";
        public string BackgroundImage { get; set; } = "";
        public string StoreName { get; set; } = "";
        public int FollowersCount { get; set; } = 0;
        public int ItemsCount { get; set; } = 0;
        public int OrdersCount { get; set; } = 0;
        public string Description { get; set; } = "";
    }
}