using MediatR;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_For_Request_
{
    public class GetItemForRequest : IRequest<Item>
    {
        public GetItemForRequest(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
}