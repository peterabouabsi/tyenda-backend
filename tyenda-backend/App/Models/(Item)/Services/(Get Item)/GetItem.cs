using MediatR;
using tyenda_backend.App.Models._Item_.Views;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_
{
    public class GetItem : IRequest<ItemAdvancedView>
    {
        public GetItem(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
}