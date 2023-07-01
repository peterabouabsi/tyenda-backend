using MediatR;
using tyenda_backend.App.Models.Form;

namespace tyenda_backend.App.Models._Store_.Services._Add_Remove_Cart_
{
    public class AddRemoveCart : IRequest<bool>
    {
        public AddRemoveCart(AddRemoveCartForm addRemoveCartForm)
        {
            AddRemoveCartForm = addRemoveCartForm;
        }

        public AddRemoveCartForm AddRemoveCartForm { get; set; }
    }
}