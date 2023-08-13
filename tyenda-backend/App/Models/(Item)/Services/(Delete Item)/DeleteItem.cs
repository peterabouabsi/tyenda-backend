using MediatR;

namespace tyenda_backend.App.Models._Item_.Services._Delete_Item_
{
    public class DeleteItem : IRequest<bool>
    {
        public DeleteItem(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
}