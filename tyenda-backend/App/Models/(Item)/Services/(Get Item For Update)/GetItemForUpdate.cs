using MediatR;

namespace tyenda_backend.App.Models._Item_.Services._Get_Item_For_Update_
{
    public class GetItemForUpdate : IRequest<object>
    {
        public GetItemForUpdate(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
}