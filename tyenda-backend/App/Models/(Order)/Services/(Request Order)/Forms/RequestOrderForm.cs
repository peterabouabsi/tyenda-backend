namespace tyenda_backend.App.Models._Order_.Services._Request_Order_.Forms
{
    public class RequestOrderForm
    {
        public string ReceiverName { get; set; } = "";
        public string ReceiverEmail { get; set; } = "";
        public string ReceiverPhone { get; set; } = "";
        public string CityId { get; set; } = "";
        public string AddressDetails { get; set; } = "";
        public string Note { get; set; } = "";
        public decimal Longitude { get; set; } = 0;
        public decimal Latitude { get; set; } = 0;
    }
}