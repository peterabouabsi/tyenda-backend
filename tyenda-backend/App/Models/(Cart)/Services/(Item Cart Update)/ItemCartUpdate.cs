using MediatR;
using tyenda_backend.App.Models._Cart_.Services._Item_Cart_Update_.Form;

namespace tyenda_backend.App.Models._Cart_.Services._Item_Cart_Update_
{
    public class ItemCartUpdate : IRequest<int>
    {
        public ItemCartUpdate(ItemCartUpdateForm itemCartUpdateForm)
        {
            ItemCartUpdateForm = itemCartUpdateForm;
        }

        public ItemCartUpdateForm ItemCartUpdateForm { get; set; }
    }
}