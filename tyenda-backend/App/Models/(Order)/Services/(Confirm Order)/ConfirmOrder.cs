using MediatR;
using tyenda_backend.App.Models._Order_.Services._Confirm_Order_.Form;

namespace tyenda_backend.App.Models._Order_.Services._Confirm_Order_
{
    public class ConfirmOrder : IRequest<Order>
    {
        public ConfirmOrder(ConfirmOrderForm confirmOrderForm)
        {
            ConfirmOrderForm = confirmOrderForm;
        }

        public ConfirmOrderForm ConfirmOrderForm { get; set; }
    }
}