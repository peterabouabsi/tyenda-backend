namespace tyenda_backend.App.Models.Form
{
    public class AddToCartForm
    {
        public string? StoreId { get; set; } = "";
        public string? ItemId { get; set; } = "";
        public int? Quantity { get; set; } = 0;
    }
}