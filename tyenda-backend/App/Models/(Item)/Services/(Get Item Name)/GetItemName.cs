using MediatR;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_Name_
{
    public class GetItemName : IRequest<string>
    {
        public GetItemName(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
}