using MediatR;
using tyenda_backend.App.Models._Order_.Services._Request_Order_.Forms;

namespace tyenda_backend.App.Models._Order_.Services._Request_Order_
{
    public class RequestOrder : IRequest<bool>
    {
        public RequestOrder(RequestOrderForm requestOrderForm)
        {
            RequestOrderForm = requestOrderForm;
        }

        public RequestOrderForm RequestOrderForm { get; set; }
    }
}