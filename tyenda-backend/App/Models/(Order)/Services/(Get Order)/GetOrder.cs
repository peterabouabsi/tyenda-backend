using MediatR;

namespace tyenda_backend.App.Models._Order_.Services._Get_Order_
{
    public class GetOrder : IRequest<Order>
    {
        public GetOrder(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}