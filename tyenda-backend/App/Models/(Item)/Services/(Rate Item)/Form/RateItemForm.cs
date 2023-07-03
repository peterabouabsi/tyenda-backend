using MediatR;

namespace tyenda_backend.App.Models._Item_.Services._Rate_Item_.Form
{
    public class RateItemForm
    {
        public string ItemId { get; set; } = "";
        public int Rate { get; set; } = 0;
    }
}