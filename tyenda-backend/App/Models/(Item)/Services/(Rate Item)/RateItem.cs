using MediatR;
using tyenda_backend.App.Models._Item_.Services._Rate_Item_.Form;

namespace tyenda_backend.App.Models._Item_.Services._Rate_Item_
{
    public class RateItem : IRequest<object>
    {
        public RateItem(RateItemForm form)
        {
            Form = form;
        }

        public RateItemForm Form { get; set; }
    }
}