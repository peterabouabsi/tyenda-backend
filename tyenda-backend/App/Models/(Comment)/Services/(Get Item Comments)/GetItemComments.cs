using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Comment_.Views;

namespace tyenda_backend.App.Models._Comment_.Services._Get_Item_Comments_
{
    public class GetItemComments : IRequest<ICollection<CommentAdvancedView>>
    {
        public GetItemComments(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
}