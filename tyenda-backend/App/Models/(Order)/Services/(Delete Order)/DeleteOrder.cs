using MediatR;

namespace tyenda_backend.App.Models._Order_.Services._Delete_Order_
{
    public class DeleteOrder : IRequest<Order>
    {
        public DeleteOrder(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}