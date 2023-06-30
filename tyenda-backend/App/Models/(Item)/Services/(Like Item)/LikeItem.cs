using MediatR;
using tyenda_backend.App.Models._Item_.Services._Like_Item_.Form;

namespace tyenda_backend.App.Models._Item_.Services._Like_Item_
{
    public class LikeItem : IRequest<bool>
    {
        public LikeItem(LikeItemForm likeItemForm)
        {
            LikeItemForm = likeItemForm;
        }

        public LikeItemForm LikeItemForm { get; set; }
    }
}