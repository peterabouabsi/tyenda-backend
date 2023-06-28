using MediatR;
using tyenda_backend.App.Models.Form;

namespace tyenda_backend.App.Models._Item_.Services._AddToCart_
{
    public class AddToCart : IRequest<bool>
    {
        public AddToCart(AddToCartForm addToCartForm)
        {
            AddToCartForm = addToCartForm;
        }

        public AddToCartForm AddToCartForm { get; set; }
    }
}