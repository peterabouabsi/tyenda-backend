using MediatR;
using tyenda_backend.App.Models._Order_.Services._Complete_Order_.Form;

namespace tyenda_backend.App.Models._Order_.Services._Complete_Order_
{
    public class CompleteOrder : IRequest<Order>
    {
        public CompleteOrder(CompleteOrderForm completeOrderForm)
        {
            CompleteOrderForm = completeOrderForm;
        }

        public CompleteOrderForm CompleteOrderForm { get; set; }
    }
}